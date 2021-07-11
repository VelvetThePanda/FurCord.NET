using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace FurCord.NET
{
	public class RestResponse
	{
		public Dictionary<string, string> Headers { get; private set; }
		public int ResponseCode { get; private set; }
		public string Content { get; private set; }

		internal RestResponse(int responseCode, string content, HttpResponseHeaders headers)
		{
			ResponseCode = responseCode;
			Content = content;
			Headers = headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value));
		}
	}
}