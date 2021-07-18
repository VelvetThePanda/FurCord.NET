using Emzi0767.Utilities;
using FurCord.NET.Entities;

namespace FurCord.NET
{
	/// <summary>
	/// Represents arguments for various guild-related events.
	/// </summary>
	public sealed class GuildCreateEventArgs : AsyncEventArgs
	{
		/// <summary>
		/// The guild that was created or joined.
		/// </summary>
		public IGuild Guild { get; internal init; }
		
		/// <summary>
		/// Whether this guild is new. 
		/// </summary>
		public bool IsNew { get; internal init; }
		
		internal GuildCreateEventArgs() { }
	}
}