using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FurCord.NET.Enums;

namespace FurCord.NET
{
	public class RestRequest
	{
		public string Route { get; private set; }
		public Dictionary<string, object> Params { get; private set; }
		public RestMethod Method { get; private set; } = RestMethod.GET;
		public Dictionary<string, string> Headers { get; private set; } = new();
		
		public Task<RestResponse> Response { get; internal set; }

		public RestRequest(string path, RestMethod method, Dictionary<string, object>? route_params = null)
		{
			Route = path;
			Method = method;
			Params = route_params ?? new();
		}

		public virtual HttpRequestMessage CreateRequestMessage(string path)
		{
			var message = new HttpRequestMessage(new(Method.ToString()), path);
			
			foreach (string item in Headers.Keys) 
				message.Headers.Add(item, Headers[item]);
			
			return message;
		}

		public string ToString() => $"REST Request: {Method} {Route}";
	}
}