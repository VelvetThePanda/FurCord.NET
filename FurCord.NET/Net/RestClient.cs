using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FurCord.NET.Enums;

namespace FurCord.NET
{
	public sealed class RestClient
	{
		private readonly HttpClient _client;
		private readonly HttpHeaders _headers;

		private readonly Regex _routeRegex = new(@":([a-z_]+)");

		private readonly RestBucket _globalBucket = new(10000, 1000, DateTime.Today + TimeSpan.FromDays(1), "global", true);
		
		
		/// <summary>
		/// hash -> bucket
		/// </summary>
		private readonly ConcurrentDictionary<string, RestBucket> _buckets = new();
		
		
		/// <summary>
		/// route -> hash
		/// </summary>
		private readonly ConcurrentDictionary<string, string> _hashes = new();

		private readonly ConcurrentDictionary<string, int> _requestQueue = new();
		
		public RestClient(Uri baseUri, string userAgent, string token)
		{
			var handler = new HttpClientHandler()
			{
				UseCookies = false,
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			};

			_client = new(handler);
			_client.BaseAddress = baseUri;
			_client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", userAgent);
			_client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
		}

		public Task DoRequestAsync(RestRequest request) => DoRequestAsync(request, null, null);
		
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
				Console.WriteLine($"Cooling down! Requeing request in {delay.TotalMilliseconds}");

				requestTcs = new();
				wait = new TaskCompletionSource();
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

















