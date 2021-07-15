using System;

namespace FurCord.NET.Net.Enums
{
	/// <summary>
	/// Represents the type of token used for authenticating with Discord.
	/// </summary>
	public enum TokenType
	{
		/// <summary>
		/// A bot token.
		/// </summary>
		Bot,
		/// <summary>
		/// A bearer token.
		/// </summary>
		Bearer,
		/// <summary>
		/// A user token. <b>Under no circumstances should this be used.</b>
		/// </summary>
		[Obsolete("User tokens should not be used, and the developers of FurCord.NET are not responsible for any account bans incurred from doing so.")]
		User
	}
}