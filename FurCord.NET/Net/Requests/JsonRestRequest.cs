using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using FurCord.NET.Enums;

namespace FurCord.NET
{
	public sealed class JsonRestRequest<T> : RestRequest
	{
		[JsonPropertyName("content")]
		public T Content { get; set; }

		public JsonRestRequest(string route, T content, RestMethod method = RestMethod.POST, Dictionary<string, object> route_params = null) 
			: base(route, method, route_params)
		{
			if (method is RestMethod.GET)
				throw new InvalidOperationException("GET requests cannot have a body.");
			
			Content = content;
		}
		
		public override HttpRequestMessage CreateRequestMessage(string path)
		{
			var request = base.CreateRequestMessage(path);
			request.Content = new StringContent(JsonSerializer.Serialize(Content));
			request.Content.Headers.ContentType = new("application/json");
			return request;
		}
	}
}