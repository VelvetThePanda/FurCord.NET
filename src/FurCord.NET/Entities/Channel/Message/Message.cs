using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FurCord.NET.Entities.Converters;
using FurCord.NET.Entities;
using FurCord.NET.Net;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	///<inheritdoc cref="IMessage"/>
	public sealed class Message : IMessage
	{
		public ulong Id { get; internal set; }

		IDiscordClient ISnowflake.Client { get; set; }

		public IUser Author { get; internal set; }

		public IReadOnlyDictionary<ulong, IUser> MentionedUsers => _mentionedUsers;

		public IReadOnlyDictionary<ulong, IChannel> MentionedChannels => _mentionedChannels;

		[JsonProperty("content")]
		public string Content { get; internal set; }
		
		[JsonProperty("timestamp")]
		public DateTimeOffset CreationTimestamp { get; internal set; }

		[JsonProperty("mentioned_users")]
		[JsonConverter(typeof(SnowflakeDictionaryConverter<User>))]
		internal ConcurrentDictionary<ulong, IUser> _mentionedUsers = new();
		
		[JsonProperty("mentioned_channels")]
		[JsonConverter(typeof(SnowflakeDictionaryConverter<Channel>))]
		internal ConcurrentDictionary<ulong, IChannel> _mentionedChannels = new();
		
	}
}