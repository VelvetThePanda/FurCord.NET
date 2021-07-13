using System;

namespace FurCord.NET.Net
{
	/// <summary>
	/// Configuration for various parts of the DiscordClient, and RestClient.
	/// </summary>
	public sealed class DiscordConfiguration
	{
		public string Token { internal get; set; }
		public Uri RestEndpointUri { internal get; set; } = new("https://discord.com/api/v9");
	}
}