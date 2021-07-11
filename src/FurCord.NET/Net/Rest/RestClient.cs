using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FurCord.NET.Enums;
using Newtonsoft.Json;

namespace FurCord.NET
{
	/// <summary>
	/// A REST-Only client. 
	/// </summary>
	public sealed class RestClient : IRestClient
	{
		//Underlying client to make requests with.
		private readonly HttpClient _client;

		private readonly HttpMessageHandler _handler;
		
		//Auth headers, typically.
		private readonly HttpHeaders _headers;
		
		//Used to replace :channel_id -> 859093960030027806
		private readonly Regex _routeRegex = new(":([a-z_]+)");
		
		// Thanks Jax <3
		private readonly RestBucket _globalBucket = new(10000, 1000, DateTime.Today + TimeSpan.FromDays(1), "global", true);

		// hash -> bucket
		private readonly ConcurrentDictionary<string, RestBucket> _buckets = new();
		
		// route -> hash
		private readonly ConcurrentDictionary<string, string> _hashes = new();

		/*
		 * TODO: Possibly add a request queue? Seems uncessary because buckets are self-contained, but it's up for consideration.
		 */

		/// <summary>
		/// Creates a default instance of a <see cref="RestClient"/>.
		/// </summary>
		/// <param name="token">The token to authorize requests with.</param>
		/// <returns>A <see cref="RestClient"/> with default parameters.</returns>
		public static RestClient CreateDefault(string token) => new(token, null);
		
		/// <summary>
		/// Constructs a new <see cref="RestClient"/> that will .
		/// </summary>
		/// <param name="token"></param>
		public RestClient(string token, HttpMessageHandler handler) : this(new("https://discord.com/api/v9/"), token, handler) { }
		
		
		/// <summary>
		/// Constructs a new <see cref="RestClient"/>.
		/// </summary>
		/// <param name="baseUri">The base path that all requests will be sent on.</param>
		/// <param name="token">The token to use for authorization.</param>
		/// <param name="handler>The handler to use for this client, or a default handler if none is specified.</param>
		public RestClient(Uri baseUri, string token, HttpMessageHandler? handler)
		{ 
			handler ??= new HttpClientHandler()
			{
				UseCookies = false,
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			};

			_client = new(handler);

			_client.BaseAddress = baseUri;
			_client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "FurCord.NET by VelvetThePanda / v0.1");
			_client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
		}
		
		
		/// <summary>
		/// Executes a REST request, or requeues and waits if necessary. The result is set on <see cref="RestRequest.Response"/> of the passed request..
		/// </summary>
		/// <param name="request">The request to requeue.</param>
		public Task DoRequestAsync(RestRequest request) => DoRequestAsync(request, null, null);

		public async Task<T> DoRequestAsync<T>(RestRequest request)
		{
			await DoRequestAsync(request, null, null);
			RestResponse response = await request.Response;

			var ret = JsonConvert.DeserializeObject<T>(response.Content);
			
			return ret;
		}
		
		internal async Task DoRequestAsync(RestRequest request, TaskCompletionSource? wait = null, TaskCompletionSource<RestResponse>? requestTcs = null)
		{
			if (wait is not null) 
				await wait.Task;

			var req = request.CreateRequestMessage(_routeRegex.Replace(request.Route, re => request.Params[re.Groups[1].Value].ToString()));
			
			_buckets.TryGetValue(request.Route, out RestBucket bucket);
			bucket ??= _globalBucket;

			var now = DateTime.UtcNow;
			if (!await bucket.CanUseAsync() && wait is null)
			{
				var delay = (bucket.ResetsAt - now) + TimeSpan.FromMilliseconds(100); //Just in case.//
				if (delay < TimeSpan.Zero)
					delay = TimeSpan.Zero;
				//TODO: ILogger
				Console.WriteLine($"Oh no, you've been ratelimited for {delay.TotalMilliseconds:F0} ms!");

				wait = new();
				requestTcs = new();
				request.Response = requestTcs.Task;

				_ = Task.Delay(delay).ContinueWith((_, t) => ((TaskCompletionSource) t).TrySetResult(), wait);
				_ = DoRequestAsync(request, wait, requestTcs);
				
				return;
			}
			
			var res = await _client.SendAsync(req);
			var content = await res.Content.ReadAsStringAsync();

			var ret = new RestResponse((int) res.StatusCode, content, res.Headers);

			if (RestBucket.TryParse(res.Headers, out bucket)) 
				_buckets.AddOrUpdate(request.Route, bucket, (_, old) => old.ResetsAt < bucket.ResetsAt ? bucket : old);

			if (requestTcs is null)
				request.Response = Task.FromResult(ret);
			else
				requestTcs.TrySetResult(ret);
		}
	}
}