using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	public interface IChannel : ISnowflake
	{
		[JsonProperty("name")]
		public string Name { get; }

		[JsonProperty("guild_id")]
		public ulong? GuildId { get; internal set; }
		
		[JsonIgnore]
		public IGuild? Guild { get; internal set; }
		
		
	}
}