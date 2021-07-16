using System;
using FurCord.NET.Net.Enums;
using Microsoft.Extensions.Logging;

namespace FurCord.NET.Net
{
	/// <summary>
	/// Configuration for various parts of the DiscordClient, and RestClient.
	/// </summary>
	public sealed class DiscordConfiguration
	{
		public string Token { get; }
		public TokenType TokenType { get; }

		public ILoggerFactory? LoggerInstanceFactory { internal get; init; }

		public GatewayIntents Intents { internal get; init; }
		
		public WebSocketClientFactoryDelegate WebSocketClientFactory
		{
			internal get => _socketClientFactory;
			init => _socketClientFactory = value ?? throw new InvalidOperationException("Delegate must be non-null.");
		}
		
		private readonly WebSocketClientFactoryDelegate _socketClientFactory;

		public RestClientFactoryDelegate RestClientFactory
		{
			internal get => _restClientFactory;
			init => _restClientFactory = value ?? throw new InvalidOperationException("Delegate must be non-null.");
		}

		private readonly RestClientFactoryDelegate _restClientFactory;

		public LogLevel MinimumLogLevel { get; init; }
		public string LogTimestampFormat { get; init; }
		public DiscordConfiguration(string token, TokenType tokenType = TokenType.Bot, ILoggerFactory? loggerInstanceFactory = null, 
			GatewayIntents intents = default, LogLevel minimumLogLevel = LogLevel.Information, 
			string logTimestampFormat = "yy-MM-dd hh:mm ss", RestClientFactoryDelegate? restClientFactory = null, WebSocketClientFactoryDelegate? socketClientFactory = null)
		{
			Token = token;
			TokenType = tokenType;
			
			LoggerInstanceFactory = loggerInstanceFactory ?? new LoggerFactory(new ILoggerProvider[]{ new BaseLoggerProvider()});
			
			Intents = intents;
			MinimumLogLevel = minimumLogLevel;
			LogTimestampFormat = logTimestampFormat;
			
			_socketClientFactory = socketClientFactory ?? WebSocketClient.CreateNew;
			_restClientFactory = restClientFactory ?? RestClient.CreateNew;
		}
	}
}