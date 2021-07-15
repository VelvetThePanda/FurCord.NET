using System.Collections.Concurrent;
using System.Collections.Generic;
using FurCord.NET.Net;
using FurCord.NET.Utils;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	///<summary>
	/// Concrete implmentation of <see cref="IGuild"/>.
	///</summary>
	/// <inheritdoc cref="IGuild"/>
	public sealed class Guild : IGuild
	{
		public ulong Id { get; internal set; }

		IDiscordClient ISnowflake.Client { get; set; }

		public string Name { get; internal set; }

		public string IconUrl => CDN.GuildIcon(Id, IconHash);

		public string IconHash { get; internal set; }

		public string SlashUrl => CDN.GuildSplash(Id, SplashHash);
		public string SplashHash { get; internal set; }
		

		public string PreferredLocale { get; internal set; }

		public ulong OwnerId { get; internal set; }

		ulong IGuild.AfkChannelId { get; set; }

		public int? MaxMembers { get; internal set; }

		public int? MaxPresences { get; internal set; }

		public int? ApproximatePresenceCount { get; internal set; }

		public IReadOnlyDictionary<ulong, IChannel> Channels => _channels;

		public IReadOnlyDictionary<ulong, IMember> Members => _members;

		public IChannel? AfkChannel => GetChanenlFromCache((this as IGuild).AfkChannelId);
		
		int IGuild.AfkTimeout { get; set; }

		public MFALevel MFALevel { get; internal set; }

		[JsonProperty("channels")]
		[JsonConverter(typeof(SnowflakeDictionaryConverter<Channel>))]
		internal ConcurrentDictionary<ulong, IChannel> _channels = new();

		[JsonProperty("members")]
		[JsonConverter(typeof(SnowflakeDictionaryConverter<Member>))]
		internal ConcurrentDictionary<ulong, IMember> _members = new();
		private IDiscordClient _client;


		/// <summary>
		/// Populates this guild's cache by setting GuildId and Guild properties appropriately.
		/// </summary>
		internal void PopulateObjects()
		{
			foreach ((_, var member) in _members!)
			{
				member.GuildId = Id;
				member.Guild = this;
			}
			
			foreach ((_, var chn) in _channels)
			{
				chn.GuildId = Id;
				chn.Guild = this;
			}
		}

		/// <summary>
		/// Convenience wrapper for <see cref="IDictionary{TKey,TValue}.TryGetValue"/>
		/// </summary>
		/// <param name="id">The Id of the channel to get.</param>
		/// <returns>The returned channel.</returns>
		private IChannel? GetChanenlFromCache(ulong id)
		{
			_channels.TryGetValue(id, out IChannel? channel);
			return channel;
		}
	}
}