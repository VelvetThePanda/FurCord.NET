using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	public interface IRole : ISnowflake
	{
		[JsonProperty("name")]
		public string Name { get; }
		
		[JsonProperty("permissions")]
		public Permissions Permissions { get; }
		
	}
}