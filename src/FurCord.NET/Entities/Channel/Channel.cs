using FurCord.NET.Net;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// Concrete implementation of <see cref="IChannel"/>
	/// </summary>
	public sealed class Channel : IChannel
	{
		public ulong Id { get; internal set; }
		
		IDiscordClient ISnowflake.Client { get; set; }

		public string Name { get; internal set; }

		ulong? IChannel.GuildId { get; set; }

		IGuild? IChannel.Guild { get; set; }

	}
}