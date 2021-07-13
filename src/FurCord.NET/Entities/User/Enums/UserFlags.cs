using System;

namespace FurCord.NET.Entities.Enums
{
	/// <summary>
	/// Flags relating to various badges* on a user's profile.
	/// <br/><br/>
	/// <sup>*not all flags coorelate to a badge.</sup>
	/// </summary>
	[Flags]
	public enum UserFlags
	{
		//TODO: Documentation
		None = 0,
		Staff = 1 << 0,
		PartneredServerOwner = 1 << 1,
		HypeSquadEvents = 1 << 2,
		BugHunterLevelOne = 1 << 3,
		HouseBravery = 1 << 6,
		HouseBrilliance = 1 << 7,
		HouseBalance = 1 << 8,
		EarlySupporter = 1 << 9,
		TeamUser = 1 << 10,
		BugHunterLevelTwo = 1 << 14,
		VerifiedBot = 1 << 16,
		EarlyVerifiedBotDeveloper = 1 << 17,
		DiscordCertifiedModerator = 1 << 18
	}
}
