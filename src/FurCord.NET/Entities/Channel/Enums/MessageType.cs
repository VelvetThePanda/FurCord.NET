namespace FurCord.NET.Entities
{
	/// <summary>
	/// Represents a type of message.
	/// </summary>
	public enum MessageType
	{
		Default,
		RecipientAdd,
		RecipientRemove,
		Call,
		GroupChannelNameChanged,
		IconChanged,
		PinnedMessage,
		GuildMemberJoined,
		GuildBoostAdded,
		GuildBoostLevelOne,
		GuildBoostLevelTwo,
		GuildBoostLevelThree,
		FollowingNewsChannel,
		GuildDiscorveryDisqualified,
		GuildDiscoveryRequalified,
		GuildDiscoveryGracePeriodInitialWarning,
		GuildDiscoveryGracePeriodFinalWarning,
		ThreadCreated,
		Reply,
		ApplicationCommand,
		ThreadStarterMessage,
		GuildInviteReminder
	}
}