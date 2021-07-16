using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FurCord.NET.Entities;
using FurCord.NET.Net.Enums;
using FurCord.NET.Utils;
using Microsoft.Extensions.Logging;

namespace FurCord.NET.Net
{
	public sealed partial class DiscordClient : IDiscordClient
	{
		public int Ping { get; private set; }
		public ClientState State { get; private set; } = ClientState.Disconnected;

		DiscordConfiguration IDiscordClient.Configuration => _configuration;

		public IUser CurrentUser { get; private set; }

		public IReadOnlyDictionary<ulong, IGuild> Guilds => _guilds;

		private readonly ConcurrentDictionary<ulong, IGuild> _guilds = new();
		
		private bool _running;
		private bool _disconnecting;

		private readonly ILogger<IDiscordClient> _logger;
		
		private readonly string _token;
		private readonly GatewayIntents _intents;
		private GatewayInfo _gatewayInfo;
		
		private CancellationTokenSource _cts;
		private CancellationToken _cancellation;

		private readonly IRestClient _rest;
		public readonly IWebSocketClient _socketClient;
		private DiscordConfiguration _configuration;


		public DiscordClient(DiscordConfiguration config)
		{
			_configuration = config;

			_logger = config.LoggerInstanceFactory.CreateLogger<IDiscordClient>();
			
			_token = Utils.Utils.GetFormattedToken(config.TokenType, config.Token);
			_intents = config.Intents;
			
			_socketClient = config.WebSocketClientFactory();
			_rest = config.RestClientFactory(config);
			
			_socketClient.Headers.TryAdd("Authorization", $"Bot {_token}");

			_socketClient.MessageReceived += HandleDispatchAsync;
			_socketClient.SocketClosed += SocketClosed;

			_cts = new();
			_cancellation = _cts.Token;
		}

		public async Task ConnectAsync()
		{
			if (_running)
				throw new InvalidOperationException("Client is already started!");
			
			_running = true;
			_disconnecting = false;
			
			var gatewayRestRequest = new RestRequest("gateway/bot", RestMethod.GET);
			_gatewayInfo = await _rest.DoRequestAsync<GatewayInfo>(gatewayRestRequest);

			var gatewayUri = new QueryUriBuilder(_gatewayInfo.Url).AddParameter("v", "8").AddParameter("encoding", "json").Build();
			
			await _socketClient.ConnectAsync(gatewayUri);
		}

		public async Task DisconnectAsync()
		{
			_disconnecting = true;
			await _socketClient.DisconnectAsync();
			_cts.Cancel();
		}

		private async Task SocketClosed(IWebSocketClient sender, SocketClosedEventArgs e)
		{
			if (_disconnecting) 
				return;
			
			_running = false;
			
			Console.WriteLine("Socket closed. Reconnecting.");
			_cts.Cancel();
			
			_cts = new();
			_cancellation = _cts.Token;
			await ConnectAsync().ConfigureAwait(false);
		}
		
		public async Task<IMessage> SendMessageAsync(IUser user, IMessage message)
		{
			return null;
		}

		
	}
}