namespace FurCord.NET.Entities
{
	public enum ApplicationFlags
	{
		GatewayPresence = 1 << 12,
		GaetwayPresenceLimited = 1 << 13,
		GatewayGuildMembers = 1 << 14,
		GatewayGuildMembersLimited = 1 << 15,
		VerificationPendingGuildLimit = 1 << 16,
		Embeded = 1 << 17
	}
}