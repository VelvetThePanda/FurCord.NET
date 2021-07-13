using FurCord.NET.Net;

namespace FurCord.NET.Entities
{
	public class Role : IRole
	{
		public ulong Id { get; internal set; }

		IDiscordClient ISnowflake.Client { get; set; }

		public string Name { get; internal set; }

		public Permissions Permissions { get; internal set; }
	}
}