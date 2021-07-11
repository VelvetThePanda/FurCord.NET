using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	public interface IGuild : ISnowflake
	{
		[JsonProperty("name")]
		public string Name { get; }
		
		public string IconUrl { get; }
		
		[JsonProperty("icon")]
		public string IconHash { get; }
		
		[JsonProperty("splash")]
		public string SlashHash { get; }
		
		public string SlashUrl { get; }
		
		[JsonProperty("preferred_locale")]
		public string PreferredLocale { get; }
		
		[JsonProperty("owner_id")]
		public ulong OwnerId { get; }
		
		[JsonProperty("afk_channel_id")]
		internal ulong AfkChannelId { get; }
		
		public IChannel? AfkChannel { get; }
		
		[JsonProperty("afk_timeout")]
		internal int AfkTimeout { get; }
		
		[JsonProperty("mfa_level")]
		public MFALevel MFALevel { get; }
	}
}