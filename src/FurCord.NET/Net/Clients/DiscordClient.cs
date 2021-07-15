using System;
using System.Threading;
using System.Threading.Tasks;
using FurCord.NET.Entities;
using FurCord.NET.Net.Enums;
using FurCord.NET.Utils;

namespace FurCord.NET.Net
{
	public sealed partial class DiscordClient : IDiscordClient
	{
		public int Ping { get; private set; }
		public ClientState State { get; private set; } = ClientState.Disconnected;
		public IUser CurrentUser { get; private set; }

		private bool _started;
		
		private readonly string _token;
		private readonly GatewayIntents _intents;
		private GatewayInfo _gatewayInfo;
		private CancellationTokenSource _cts = new();
		private CancellationToken _cancellation;
		
		internal IRestClient Rest { get; set; }
		internal IWebSocketClient SocketClient { get; set; }

		
		public DiscordClient(DiscordConfiguration config)
		{
			_token = StringUtils.GetFormattedToken(config.TokenType, config.Token);
			_intents = config.Intents;
			
			SocketClient = config.WebSocketClientFactory();
			Rest = config.RestClientFactory(config);
			
			SocketClient.Headers.TryAdd("Authorization", $"Bot {_token}");

			SocketClient.MessageReceived += HandleDispatch;
			
		}
		
		public async Task ConnectAsync()
		{
			if (_started)
				throw new InvalidOperationException("Client is already started!");
			_started = true;
			
			var gatewayRestRequest = new RestRequest("gateway/bot", RestMethod.GET);
			_gatewayInfo = await Rest.DoRequestAsync<GatewayInfo>(gatewayRestRequest);

			var gatewayUri = new QueryUriBuilder(_gatewayInfo.Url).AddParameter("v", "8").AddParameter("encoding", "json").Build();
			
			await SocketClient.ConnectAsync(gatewayUri);

		}
		
		public async Task DisconnectAsync() { }
		
		
		public async Task<IMessage> SendMessageAsync(IUser user, IMessage message)
		{
			return null;
		}

		
	}
}