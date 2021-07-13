namespace FurCord.NET.Entities
{
	/// <summary>
	/// Concrete implementation of <see cref="IChannel"/>
	/// </summary>
	public class Channel : IChannel
	{
		public ulong Id { get; internal set; }

		public string Name { get; internal set; }

		ulong? IChannel.GuildId { get; set; }

		IGuild? IChannel.Guild { get; set; }

	}
}