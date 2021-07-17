using FurCord.NET.Entities.Converters;
using FurCord.NET.Net;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// A member of a <see cref="DiscordTeam"/>
	/// </summary>
	public sealed class DiscordTeamMember : ISnowflake
	{
		public ulong Id => User.Id;

		IDiscordClient ISnowflake.Client { get; set; }
		
		/// <summary>
		/// The user of this team member.
		/// </summary>
		[JsonProperty("user")]
		[JsonConverter(typeof(ConcreteTypeConverter<User>))]
		public IUser User { get; internal set; }
		
		/// <summary>
		/// The state of this member's invitation.
		/// </summary>
		[JsonProperty("membership_state")]
		public TeamInviteMembershipState MembershipState { get; internal set; }
		
		/// <summary>
		/// The permissions of this team member. Will always be "*".
		/// </summary>
		[JsonProperty("permissions")]
		public string[] Permissions { get; internal set; }
	}
}