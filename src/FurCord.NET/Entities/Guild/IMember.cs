using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// A member of an <see cref="IGuild"/>.
	/// </summary>
	public interface IMember : IUser
	{
		/// <summary>
		/// The member's set nickname, if any.
		/// </summary>
		[JsonProperty("nick")]
		public string? Nickname { get; }
		
		/// <summary>
		/// Get's the name displayed for the member. If they have a nickname, else their username.
		/// </summary>
		[JsonIgnore]
		public string DisplayName => Nickname ?? Username;
		
		[JsonProperty("avatar")]
		public string? GuildAvatarHash { get; }
		
		/// <summary>
		/// The Id of the guild this member belongs to.
		/// </summary>
		[JsonIgnore]
		public ulong GuildId { get; internal set; }
		
		/// <summary>
		/// The guild this member belongs to.
		/// </summary>
		[JsonIgnore]
		public IGuild Guild { get; internal set; }
		
	}
}