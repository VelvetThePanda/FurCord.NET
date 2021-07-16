using System.Text;
using FurCord.NET.Net.Enums;

namespace FurCord.NET.Utils
{
	internal static class StringUtils
	{
		/// <summary>
		/// Gets a formatted token for authenticating with Discord.
		/// </summary>
		/// <param name="type">The type of token.</param>
		/// <param name="token">The unformatted token.</param>
		/// <returns>A token prefixed with the appropriate identifier.</returns>
		public static string GetFormattedToken(TokenType type, string token)
			=> type switch
			{
				TokenType.Bot => $"Bot {token}",
				TokenType.Bearer => $"Bearer {token}",
				TokenType.User => token
			};

		public static UTF8Encoding UTF8 { get; } = new(false);
	}
}