using System;
using FurCord.NET.Entities.Converters;
using FurCord.NET.Entities;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	///<inheritdoc cref="IMessage"/>
	public class Message : IMessage
	{
		[JsonProperty("id")]
		public ulong Id { get; internal set; }
		
		public IUser Author { get; internal set; }
		
		[JsonProperty("content")]
		public string Content { get; internal set; }
		
		[JsonProperty("timestamp")]
		public DateTimeOffset CreationTimestamp { get; internal set; }
	}
}