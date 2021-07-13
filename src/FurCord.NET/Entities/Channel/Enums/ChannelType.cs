namespace FurCord.NET.Entities
{
	/// <summary>
	/// What type a channel is.
	/// </summary>
	public enum ChannelType
	{
		/// <summary>
		/// A normal text channel.
		/// </summary>
		Text = 0,
		/// <summary>
		/// A DM.
		/// </summary>
		Private = 1,
		/// <summary>
		/// A voice channel.
		/// </summary>
		Voice = 2,
		/// <summary>
		/// A group DM.
		/// </summary>
		Group = 3,
		/// <summary>
		/// A category which houses other channels.
		/// </summary>
		Category = 4,
		/// <summary>
		/// A News/Announcement channel.
		/// </summary>
		News = 5,
		/// <summary>
		/// A store channel.
		/// </summary>
		Store = 6,
		/// <summary>
		/// A thread formed in a news channel.
		/// </summary>
		NewsThread = 10,
		/// <summary>
		/// A public thread in a normal text channel that anyone can join.
		/// </summary>
		PublicThread = 11,
		/// <summary>
		/// A private thread in a text channel that select people can join.
		/// </summary>
		PrivateThread = 12,
		/// <summary>
		/// A stage channel.
		/// </summary>
		Stage = 13,
	}
}