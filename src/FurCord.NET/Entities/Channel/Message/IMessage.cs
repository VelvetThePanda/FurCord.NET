using System;
using System.Collections.Generic;
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
		/// The users mentioned in this message.
		/// </summary>
		public IReadOnlyDictionary<ulong, IUser> MentionedUsers { get; }
		
		/// <summary>
		/// The channels mentioned in this message.
		/// </summary>
		public IReadOnlyDictionary<ulong, IChannel> MentionedChannels { get; }
		
		/// <summary>
		/// When this message was created.
		/// </summary>
		public DateTimeOffset CreationTimestamp { get; }
	}
}