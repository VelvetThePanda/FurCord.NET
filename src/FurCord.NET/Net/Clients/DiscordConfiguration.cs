using System;
using FurCord.NET.Net.Enums;

namespace FurCord.NET.Net
{
	/// <summary>
	/// Configuration for various parts of the DiscordClient, and RestClient.
	/// </summary>
	public sealed class DiscordConfiguration
	{
		public string Token { get; }
		public TokenType TokenType { get; }
		
		public GatewayIntents Intents { internal get; init; }
		
		public WebSocketClientFactoryDelegate WebSocketClientFactory
		{
			internal get => _socketClientFactory;
			init => _socketClientFactory = value ?? throw new InvalidOperationException("Delegate must be non-null.");
		}
		
		private readonly WebSocketClientFactoryDelegate _socketClientFactory;
		
		public DiscordConfiguration(string token, TokenType tokenType = TokenType.Bot, 
			GatewayIntents intents = GatewayIntents.AllUnprivileged,
			RestClientFactoryDelegate? restClientFactory = null, WebSocketClientFactoryDelegate? socketClientFactory = null)
		{
			Token = token;
			Intents = intents;
			TokenType = tokenType;
			_socketClientFactory = socketClientFactory ?? WebSocketClient.CreateNew;
		}
	}
}