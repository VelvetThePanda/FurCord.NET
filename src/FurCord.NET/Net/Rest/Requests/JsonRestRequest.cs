using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FurCord.NET
{
	/// <summary>
	/// Represents a REST request that contains an object serialized to JSON.
	/// </summary>
	/// <typeparam name="T">The type to deserialize.</typeparam>
	public sealed class JsonRestRequest<T> : JsonRestRequest
	{
		/// <summary>
		/// The content of this request.
		/// </summary>
		[JsonPropertyName("content")]
		public override object Content { get; internal set; }
		
		public override string SerializedContent { get; internal set; }

		/// <summary>
		/// Constructs a new <see cref="JsonRestRequest{T}"/>.
		/// </summary>
		/// <param name="route">The non-parameterized route.</param>
		/// <param name="content">The content to send with this request.</param>
		/// <param name="method">The method this request should use to send.</param>
		/// <param name="route_params">Parameters to swap placeholders (e.g. :channel_id) for.</param>
		/// <exception cref="InvalidOperationException">Thrown if <see cref="RestMethod.GET"/> is attempted to be used.</exception>
		public JsonRestRequest(string route, T content, RestMethod method = RestMethod.POST, Dictionary<string, object>? route_params = null) 
			: base(route, method, route_params)
		{
			if (method is RestMethod.GET)
				throw new InvalidOperationException("GET requests cannot have a body.");
			
			Content = content;
			SerializedContent = JsonConvert.SerializeObject(content);
		}
		
		/// <inheritdoc cref="RestRequest.CreateRequestMessage"/>
		public override HttpRequestMessage CreateRequestMessage(string path)
		{
			var request = base.CreateRequestMessage(path);
			request.Content = new StringContent(SerializedContent);
			request.Content.Headers.ContentType = new("application/json");
			return request;
		}
	}

	public class JsonRestRequest : RestRequest
	{
		public virtual object Content { get; internal set; }
		public virtual string SerializedContent { get; internal set; }
		public JsonRestRequest(string path, RestMethod method, Dictionary<string, object>? route_params = null) : base(path, method, route_params) { }
	}
}