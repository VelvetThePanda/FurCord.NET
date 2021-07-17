namespace FurCord.NET.Entities
{
	/// <summary>
	/// Represents the state of a Discord Team invitation.
	/// </summary>
	public enum TeamInviteMembershipState
	{
		/// <summary>
		/// This member has been invited to the team, but has not accepted yet.
		/// </summary>
		Invited,
		/// <summary>
		/// This member has accepted the team invite.
		/// </summary>
		Accepted
	}
}