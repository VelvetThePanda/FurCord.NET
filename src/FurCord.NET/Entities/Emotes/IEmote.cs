using System.Collections.Generic;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// An emote.
	/// </summary>
	public interface IEmote : ISnowflake
	{
		/// <summary>
		/// The Id of this emote, or 0 if it is a unicode emoji.
		/// </summary>
		public ulong Id { get; }

		/// <summary>
		/// Whether this emote is managed by an integration.
		/// </summary>
		public bool IsManaged { get; }
		
		/// <summary>
		/// Whether this emote requires colons 
		/// </summary>
		public bool RequiresColons { get; }
		
		/// <summary>
		/// Roles snowflakes that are permitted to use this emote.
		/// </summary>
		public IReadOnlyList<ulong> Roles { get; }
		
		/// <summary>
		/// 
		/// </summary>
		public ulong? Owner { get; }
	}
}