using System.Collections.Generic;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	//[JsonConverter(typeof(ConcreteTypeConverter<Guild>))]
	public interface IGuild : ISnowflake
	{
		[JsonProperty("name")]
		public string Name { get; }
		
		[JsonIgnore]
		public string IconUrl { get; }
		
		[JsonProperty("icon")]
		public string IconHash { get; }
		
		[JsonProperty("splash")]
		public string SplashHash { get; }
				
		[JsonIgnore]
		public string SlashUrl { get; }
		
		[JsonProperty("preferred_locale")]
		public string PreferredLocale { get; }
		
		[JsonProperty("owner_id")]
		public ulong OwnerId { get; }
		
		[JsonProperty("afk_channel_id")]
		internal ulong AfkChannelId { get; set; }
		
		[JsonProperty("max_members")]
		public int? MaxMembers { get; }
		
		[JsonProperty("max_presences")]
		public int? MaxPresences { get; }
		
		[JsonProperty("approximate_presence_count")]
		public int? ApproximatePresenceCount { get; }
				
		[JsonIgnore]
		public IReadOnlyDictionary<ulong, IChannel> Channels { get; }
				
		[JsonIgnore]
		public IReadOnlyDictionary<ulong, IMember> Members { get; }
		
		[JsonIgnore]
		public IChannel? AfkChannel { get; }
		
		[JsonProperty("afk_timeout")]
		internal int AfkTimeout { get; set; }

		[JsonProperty("mfa_level")]
		public MFALevel MFALevel { get; }
	}
}