namespace FurCord.NET.Entities
{
	/// <summary>
	/// The "nfsw-level" of a server.
	/// </summary>
	public enum NsfwLevel
	{
		/// <summary>
		/// This server is marked as safe, and anyone can join.
		/// </summary>
		Safe,
		
		/// <summary>
		/// This server is age-restricted, but not strictly nsfw.
		/// </summary>
		AgeRestricted,
		
		/// <summary>
		/// This server is nsfw and will be blocked by default on iOS devices.
		/// </summary>
		Nsfw
	}
}