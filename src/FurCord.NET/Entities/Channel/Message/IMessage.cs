using System;
using FurCord.NET.Entities.Converters;
using FurCord.NET.Entities;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// A Discord message.
	/// </summary>
	//[JsonConverter(typeof(ConcreteTypeConverter<Message>))]
	public interface IMessage : ISnowflake
	{
		/// <summary>
		/// The content of this message, if any.
		/// </summary>
		public string Content { get; }
		
		[JsonProperty("author")]
		[JsonConverter(typeof(ConcreteTypeConverter<User>))]
		public IUser Author { get; }
		
		/// <summary>
		/// When this message was created.
		/// </summary>
		public DateTimeOffset CreationTimestamp { get; }
	}
}