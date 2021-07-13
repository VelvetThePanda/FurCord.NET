using System;
using Microsoft.Extensions.Logging;

namespace FurCord.NET.Net
{
	/// <summary>
	/// Configuration for various parts of the DiscordClient, and RestClient.
	/// </summary>
	public sealed class DiscordConfiguration
	{
		public string Token { internal get; set; }

		public ILoggerFactory LoggerFactory { internal get; set; } = new LoggerFactory();

	}
}