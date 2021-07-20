using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FurCord.NET.Entities;
using FurCord.NET.Net.Enums;
using FurCord.NET.Net.Payloads;
using FurCord.NET.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FurCord.NET.Net
{
	internal sealed partial class DiscordClient : IDiscordClient
	{
		public int Ping { get; private set; }
		
		public IUser CurrentUser { get; private set; }
		public ClientState State { get; private set; } = ClientState.Disconnected;
		
		DiscordConfiguration IDiscordClient.Configuration => _configuration;
		
		private readonly DiscordConfiguration _configuration;
		
		public IReadOnlyDictionary<ulong, IGuild> Guilds => _guilds;
		private readonly ConcurrentDictionary<ulong, IGuild> _guilds = new();
		
		private bool _running;
		private bool _initialized;
		private bool _disconnecting;

		private readonly ILogger<IDiscordClient> _logger;
		
		private readonly string _token;
		private GatewayInfo _gatewayInfo;
		private readonly GatewayIntents _intents;

		private CancellationTokenSource _cts;
		private CancellationToken _cancellation;

		private readonly IRestClient _rest;
		public readonly IWebSocketClient _socketClient;
		
		public DiscordClient(DiscordConfiguration config, ILogger<DiscordClient> logger, IWebSocketClient socket, IRestClient rest)
		{
			_configuration = config;

			_logger = logger;
			
			_token = Utils.Utils.GetFormattedToken(config.TokenType, config.Token);
			_intents = config.Intents;
			
			_rest = rest;
			_socketClient = socket;
			
			_socketClient.Headers.TryAdd("Authorization", $"Bot {_token}");

			_socketClient.MessageReceived += HandleDispatchAsync;
			_socketClient.SocketClosed += SocketClosed;

			_cts = new();
			_cancellation = _cts.Token;
		}

		public async Task ConnectAsync()
		{
			if (_running)
				throw new InvalidOperationException("This client is already connected!");
			
			_running = true;
			_disconnecting = false;

			if (_gatewayInfo is null)
			{
				var gatewayRestRequest = new RestRequest("gateway/bot", RestMethod.GET);
				_gatewayInfo = await _rest.DoRequestAsync<GatewayInfo>(gatewayRestRequest);
				CurrentUser = await GetCurrentUserAsync();
			}
			
			var gatewayUri = new QueryUriBuilder(_gatewayInfo.Url).AddParameter("v", "8").AddParameter("encoding", "json").Build();
			
			await _socketClient.ConnectAsync(gatewayUri);
			State = ClientState.Connected;
		}

		public async Task DisconnectAsync()
		{
			_disconnecting = true;
			await _socketClient.DisconnectAsync();
			_cts.Cancel();
		}

		private async Task SocketClosed(IWebSocketClient sender, SocketClosedEventArgs e)
		{
			State = ClientState.Disconnected;
			
			if (_disconnecting) 
				return;
			
			_running = false;

			_logger.LogError("Socket closed. Reconnecting.");
			_cts.Cancel();
			
			_cts = new();
			_cancellation = _cts.Token;
			
			await ConnectAsync().ConfigureAwait(false);
		}
		
		/// <summary>
		/// Gets a <see cref="IUser"/> based on the currently authenticated user.
		/// </summary>
		/// <returns>The current user.</returns>
		private async Task<IUser> GetCurrentUserAsync()
		{
			var request = new RestRequest("users/@me", RestMethod.GET);
			var result = await _rest.DoRequestAsync<User>(request);

			(result as ISnowflake).Client = this;
			return result;
		}
		
		public async Task<IMessage> SendMessageAsync(IUser user, string content)
		{
			var payload = new RESTMessageCreatePayload() {Content = content};
			
			var chnRequest = new JsonRestRequest<RESTCreateDMPayload>(Routes.CreateDM, new(user.Id), RestMethod.POST);
			var channel = await _rest.DoRequestAsync<Channel>(chnRequest);

			var msgRequest = new JsonRestRequest<RESTMessageCreatePayload>(Routes.Messages, payload, RestMethod.POST, new() {["channel_id"] = channel.Id});
			var message = await _rest.DoRequestAsync<Message>(msgRequest);

			return message;
		}

		
	}
}