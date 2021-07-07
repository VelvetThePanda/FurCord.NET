using System.Net.Http;

namespace FurCord.NET
{
	public class RestResponse
	{
		public int ResponseCode { get; private set; }
		public string Content { get; private set; }

		public RestResponse(HttpResponseMessage httpResponseMessage, string content)
		{
			ResponseCode = (int)httpResponseMessage.StatusCode;
			Content = content;
		}
	}
}