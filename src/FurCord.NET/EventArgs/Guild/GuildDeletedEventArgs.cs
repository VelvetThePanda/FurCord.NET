using Emzi0767.Utilities;
using FurCord.NET.Entities;

namespace FurCord.NET
{
	/// <summary>
	/// Event arguments for when a guild is deleted.
	/// </summary>
	public sealed class GuildDeletedEventArgs : AsyncEventArgs
	{
		/// <summary>
		/// The guild that was deleted.
		/// </summary>
		public IGuild Guild { get; internal init; }

		/// <summary>
		/// Whether this guild is unavailble. True during outages.
		/// </summary>
		public bool Unavailable { get; internal init; }
		internal GuildDeletedEventArgs() { }
	}
}