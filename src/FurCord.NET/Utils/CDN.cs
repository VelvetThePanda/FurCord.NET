namespace FurCord.NET.Utils
{
	/// <summary>
	/// Various Discord CDN enpoints.
	/// </summary>
	internal static class CDN
	{
		public static string GuildIcon(ulong id, string hash) => $"https://cdn.discordapp.com/icons/{id}/{hash}.png";

		public static string GuildSplash(ulong id, string hash) => $"https://cdn.discordapp.com/splashes/{id}/{hash}.png";

		public static string GuildDiscoverySplash(ulong id, string hash) => $"https://cdn.discordapp.com/discovery-splashes/{id}/{hash}.png";
		
	}
}