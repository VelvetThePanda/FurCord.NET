using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FurCord.NET
{
	public sealed class RestClient
	{
		private readonly HttpClient _client;
		private readonly HttpHeaders _headers;
		
		public RestClient(Uri baseUri, string userAgent)
		{
			var handler = new HttpClientHandler()
			{
				UseCookies = true,
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			};

			_client = new(handler);
			_client.BaseAddress = baseUri;
			_client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", userAgent);
		}
	}
}