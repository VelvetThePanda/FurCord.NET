using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FurCord.NET;

namespace FurCord.NET
{
	/// <summary>
	/// Represents a REST request.
	/// </summary>
	public class RestRequest
	{
		/// <summary>
		/// The route this request targets. e.g. channels/:channel_id/messages
		/// </summary>
		public string Route { get; private set; }
		
		/// <summary>
		/// The parameters for this request. e.g. channel_id = 859093960030027806
		/// </summary>
		public Dictionary<string, object> Params { get; private set; }
		
		/// <summary>
		/// The HTTP method of this request. GET, POST, PATCH, etc.
		/// </summary>
		public RestMethod Method { get; private set; } = RestMethod.GET;
		
		/// <summary>
		/// Any additional headers to be sent with this request.
		/// </summary>
		public Dictionary<string, string> Headers { get; set; } = new();
		
		/// <summary>
		/// This task represents the result of the request.
		///<br/>
		/// If a ratelimit is incurred, this may be the result of a TaskCompletionSource and the request will be requed, which will set the result of this.
		/// </summary>
		public Task<RestResponse> Response { get; internal set; }

		
		/// <summary>
		/// Constructs a new <see cref="RestRequest"/>.
		/// </summary>
		/// <param name="path">The route this request takes. This should be non-parmeratized. e.g. channels/:channel_id/messages</param>
		/// <param name="method">The method this request should use.</param>
		/// <param name="route_params">The parameters to swap in place for the route (e.g. :channel_id).</param>
		public RestRequest(string path, RestMethod method, Dictionary<string, object>? route_params = null)
		{
			Route = path;
			Method = method;
			Params = route_params ?? new();
		}
		
		/// <summary>
		/// Constructs the message to be sent, adding any headers or content as necessary.
		/// </summary>
		/// <param name="path">The route this request is going to be sent to.</param>
		/// <returns></returns>
		public virtual HttpRequestMessage CreateRequestMessage(string path)
		{
			var message = new HttpRequestMessage(new(Method.ToString()), path);
			
			foreach (string item in Headers.Keys) 
				message.Headers.Add(item, Headers[item]);
			
			return message;
		}
	}
}