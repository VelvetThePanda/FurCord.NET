using System.Collections.Generic;
using Emzi0767.Utilities;
using FurCord.NET.Entities;

namespace FurCord.NET
{
	/// <summary>
	/// Event arguments for when all guilds have 
	/// </summary>
	public sealed class GuildDownloadCompleteArgs : AsyncEventArgs
	{
		/// <summary>
		/// The available guilds at the time.
		/// </summary>
		public IReadOnlyDictionary<ulong, IGuild> Guilds { get; }

		internal GuildDownloadCompleteArgs(IReadOnlyDictionary<ulong, IGuild> guilds) => Guilds = guilds;
	}
}