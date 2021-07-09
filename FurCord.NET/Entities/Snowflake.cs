using System;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// Represents a unique Discord object.
	/// </summary>
	public abstract class Snowflake
	{
		/// <summary>
		/// The id of this snowflake.
		/// </summary>
		public ulong Id { get; internal set; }
		
		/// <summary>
		/// Gets when this snowflake was created.
		/// </summary>
		public DateTimeOffset CreationDate => DiscordEpoch.AddSeconds(Id << 22);
		//TODO: Add client.

		private static readonly DateTimeOffset DiscordEpoch = new DateTimeOffset(2015, 1, 1, 0, 0, 0, TimeSpan.Zero);
	}
}