using System;
using FurCord.NET.Net;
using FurCord.NET.Net.Enums;
using Microsoft.Extensions.Logging;

namespace FurCord.NET.Net
{
	/// <summary>
	/// Configuration for various parts of the DiscordClient, and RestClient.
	/// </summary>
	public sealed class DiscordConfiguration
	{
		public string Token { internal get; set; }
		public TokenType TokenType { internal get; set; }

		public ILoggerFactory LoggerFactory { internal get; set; } = new LoggerFactory();

		public GatewayIntents Intents { internal get; set; } = GatewayIntents.AllUnprivileged;
		
		public WebSocketClientFactoryDelegate WebSocketClientFactory
		{
			internal get => _socketClientFactory;
			set => _socketClientFactory = value ?? throw new InvalidOperationException("Delegate must be non-null.");
		}
		
		private WebSocketClientFactoryDelegate _socketClientFactory = WebSocketClient.CreateNew;

		public RestClientFactoryDelegate RestClientFactory
		{
			internal get => _restClientFactory;
			set => _restClientFactory = value ?? throw new InvalidOperationException("Delegate must be non-null.");
		}
		
		private RestClientFactoryDelegate _restClientFactory = RestClient.CreateNew;
	}
}