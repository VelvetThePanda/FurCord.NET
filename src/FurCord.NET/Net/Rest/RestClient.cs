using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FurCord.NET.Net;
using Newtonsoft.Json;

namespace FurCord.NET
{
	/// <summary>
	/// A REST-Only client. 
	/// </summary>
	/// <inheritdoc cref="IRestClient"/>
	public sealed class RestClient : IRestClient
	{
		//Underlying client to make requests with.
		private readonly HttpClient _client;
		
		//Used to replace :channel_id -> 859093960030027806
		private readonly Regex _routeRegex = new(":([a-z_]+)");
		
		// Thanks Jax <3
		private readonly RestBucket _globalBucket = new(10000, 10000, DateTime.Today + TimeSpan.FromDays(1), "global", true);

		// route -> bucket
		private readonly ConcurrentDictionary<string, RestBucket> _buckets = new();

		private readonly DiscordConfiguration _config;
		
		/* TODO: Possibly add a request queue? Seems uncessary because buckets are self-contained, but it's up for consideration. */

		/// <summary>
		/// Creates a default instance of a <see cref="RestClient"/>.
		/// </summary>
		/// <param name="token">The token to authorize requests with.</param>
		/// <returns>A <see cref="RestClient"/> with default parameters.</returns>
		public static RestClient CreateDefault(string token) => new(token, null);

		/// <inheritdoc />
		/// <summary>
		/// Constructs a new <see cref="T:FurCord.NET.RestClient" /> with a specified handler.
		/// </summary>
		/// <param name="token">The token to use for authorization.</param>
		/// <param name="handler">The handler to use for HTTP traffic.</param>
		internal RestClient(string token, HttpMessageHandler? handler) : this(new("https://discord.com/api/v9/"), token, handler) { }
		
		public RestClient(DiscordConfiguration config)
		{
			var handler = new HttpClientHandler()
			{
				UseCookies = false,
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			};
			_config = config;
			
			_client = new(handler) {BaseAddress = _config.RestEndpointUri};

			_client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _config.Token);
			_client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "FurCord.NET by VelvetThePanda / v0.1");
			
		}

		/// <summary>
		/// Constructs a new <see cref="RestClient"/>.
		/// </summary>
		/// <param name="baseUri">The base path that all requests will be sent on.</param>
		/// <param name="token">The token to use for authorization.</param>
		/// <param name="handler">The handler to use for this client, or a default handler if none is specified.</param>
		internal RestClient(Uri baseUri, string token, HttpMessageHandler? handler)
		{ 
			handler ??= new HttpClientHandler()
			{
				UseCookies = false,
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			};

			_client = new(handler) {BaseAddress = baseUri};

			_client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "FurCord.NET by VelvetThePanda / v0.1");
			_client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
		}
		
		
		/// <inheritdoc />
		/// <summary>
		/// Executes a REST request, or requeues and waits if necessary. The result is set on <see cref="P:FurCord.NET.RestRequest.Response" /> of the passed request..
		/// </summary>
		/// <param name="request">The request to requeue.</param>
		public Task DoRequestAsync(RestRequest request) => ExecuteRequestAsync(request);

		public async Task<T> DoRequestAsync<T>(RestRequest request)
		{
			await ExecuteRequestAsync(request);
			RestResponse response = await request.Response;

			if (response.ResponseCode == (int) HttpStatusCode.NoContent)
				return default!;

			var ret = JsonConvert.DeserializeObject<T>(response.Content);
			
			return ret!;
		}

		private async Task ExecuteRequestAsync(RestRequest request, TaskCompletionSource? wait = null, TaskCompletionSource<RestResponse>? requestTcs = null)
		{
			if (wait is not null) 
				await wait.Task;

			var req = request.CreateRequestMessage(_routeRegex.Replace(request.Route, re => request.Params[re.Groups[1].Value].ToString()!));
			
			_buckets.TryGetValue(request.Route, out RestBucket? bucket);
			bucket ??= _globalBucket;

			var now = DateTime.UtcNow;
			if (!await bucket.CanUseAsync() && wait is null)
			{
				var delay = (bucket.ResetsAt - now) + TimeSpan.FromMilliseconds(100); //Just in case.//
				if (delay < TimeSpan.Zero)
					delay = TimeSpan.Zero;
				//TODO: ILogger
				Console.WriteLine($"Cooling down! Bucket resets in {delay.TotalMilliseconds:F0} ms.");

				wait = new();
				requestTcs = new();
				request.Response = requestTcs.Task;

				_ = Task.Delay(delay).ContinueWith((_, t) => ((TaskCompletionSource) t!).TrySetResult(), wait);
				_ = ExecuteRequestAsync(request, wait, requestTcs);
				
				return;
			}
			
			var res = await _client.SendAsync(req);
			var content = await res.Content.ReadAsStringAsync();

			var ret = new RestResponse((int) res.StatusCode, content, res.Headers);

			if (RestBucket.TryParse(res.Headers, out bucket)) 
				_buckets.AddOrUpdate(request.Route, bucket, (_, old) => old.ResetsAt < bucket.ResetsAt ? bucket : old);
			
			//TODO: Handle API Bans (aka getting a 429 on the first request)
			
			if (requestTcs is null)
				request.Response = Task.FromResult(ret);
			else
				requestTcs.TrySetResult(ret);
		}
	}
}