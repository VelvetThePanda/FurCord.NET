namespace FurCord.NET.Utils
{
	/// <summary>
	/// Various Discord CDN enpoints.
	/// </summary>
	internal static class CDN
	{
		private const string CDNUrl = "https://cdn.discordapp.com";

		public static string Emoji(ulong id, bool animated = false)
			=> $"{CDNUrl}/{id}{(animated ? ".gif" : ".png")}?size=1024";
		
		/// <summary>
		/// A guild's icon.
		/// </summary>
		/// <param name="id">The id of the guild.</param>
		/// <param name="hash">The hash of the guild's icon.</param>
		/// <returns>A formatted CDN link.</returns>
		public static string GuildIcon(ulong id, string hash) 
			=> $"{CDNUrl}/icons/{id}/{(hash.StartsWith("a_") ? ".gif" : ".png")}?size=1024";

		/// <summary>
		/// A guild's splash image.
		/// </summary>
		/// <param name="id">The id of the guild.</param>
		/// <param name="hash">The hash of the guild's splash.</param>
		/// <returns>A formatted CDN link.</returns>
		public static string GuildSplash(ulong id, string hash) 
			=> $"{CDNUrl}/splashes/{id}/{hash}.png?size=1024";
		
		/// <summary>
		/// A guild's splash image in the disovery page.
		/// </summary>
		/// <param name="id">The id of the guild.</param>
		/// <param name="hash">The hash of the guild's dicovery splash.</param>
		/// <returns>A formatted CDN link.</returns>
		public static string GuildDiscoverySplash(ulong id, string hash)
			=> $"{CDNUrl}/discovery-splashes/{id}/{hash}.png?size=1024";
		
		/// <summary>
		/// A guild's banner.
		/// </summary>
		/// <param name="id">The id of the guild.</param>
		/// <param name="hash">The hash of the guild's banner.</param>
		/// <returns>A formatted CDN link.</returns>
		public static string GuildBanner(ulong id, string hash)
			=> $"{CDNUrl}/banners/{id}/{hash}.png?size=1024";

		/// <summary>
		/// The default Discord avatar, appropriately colored based on discriminator.
		/// </summary>
		/// <param name="id">The id of the user.</param>
		/// <param name="discriminator">The user's discriminator.</param>
		/// <returns>A formatted CDN link.</returns>
		public static string DefaultAvatar(ulong id, short discriminator)
			=> $"{CDNUrl}/avatars/{id}/{discriminator % 5}.png"; // Size is ignored. https://discord.dev/reference#image-formatting-cdn-endpoints
		
		/// <summary>
		/// A user's avatar.
		/// </summary>
		/// <param name="id">The id of the user.</param>
		/// <param name="hash">The hash of the user's avatar.</param>
		/// <returns>A formatted CDN link.</returns>
		public static string UserAvatar(ulong id, string hash)
			=> $"{CDNUrl}/avatars/{id}/{hash}{(hash.StartsWith("a_") ? ".gif" : ".png")}?size=1024";
		
		/// <summary>
		/// A user's guild-specific avatar.
		/// </summary>
		/// <param name="guildId">The id of the guild the user is on.</param>
		/// <param name="userId">The id of teh user.</param>
		/// <param name="guildAvatarHash">The hash of the user's avatar.</param>
		/// <returns>A formatted CDN link.</returns>
		public static string GuildAvatar(ulong guildId, ulong userId, string guildAvatarHash) =>
			$"{CDNUrl}/{guildId}/users/{userId}/{guildAvatarHash}{(guildAvatarHash.StartsWith("a_") ? ".gif" : ".png")}?size=1024";
	}
}