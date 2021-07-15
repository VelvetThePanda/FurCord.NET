using System;
using System.Threading.Tasks;
using FurCord.NET.Net.Enums;
using FurCord.NET.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FurCord.NET.Net
{
	public partial class DiscordClient
	{
		private long _lastSequence;
		private string _sessionId;
		private int _heartbeatInterval;

		private DateTime _lastHeartbeat;

		private int _skippedHeartbeats;

		private async Task HeartbeatLoopAsync()
		{
			while (!_cancellation.IsCancellationRequested)
			{
				try
				{
					_skippedHeartbeats++;

					State = _skippedHeartbeats > 5 ? ClientState.Zombied : State;
					
					var heartbeat = new GatewayPayload
					{
						OpCode = GatewayOpCode.Heartbeat,
						Data = _lastSequence
					};
					_lastHeartbeat = DateTime.Now;
					await _socketClient.SendMessageAsync(JsonConvert.SerializeObject(heartbeat));
					await Task.Delay(_heartbeatInterval, _cancellation);
				}
				catch (TaskCanceledException) { }
			}
		}
		
		private async Task HandleDispatch(IWebSocketClient client, SocketMessageEventArgs args)
		{
			var payload = JsonConvert.DeserializeObject<GatewayPayload>(args.Message)!;

			var dispatchTask = payload.OpCode switch
			{
				GatewayOpCode.Dispatch => Task.CompletedTask,
				GatewayOpCode.Reconnect => throw new NotSupportedException(),
				GatewayOpCode.Hello => OnHelloAsync((payload.Data as JObject)!.ToObject<HelloPayload>()!),
				GatewayOpCode.HeartbeatAck => AckHeartBeatAsync(),
			};
			_lastSequence = payload.Sequence ?? _lastSequence;
			await dispatchTask;
		}
		
		/// <summary>
		/// Acknowledges 
		/// </summary>
		private async Task AckHeartBeatAsync()
		{
			_skippedHeartbeats--;
			Ping = (int)(DateTime.Now - _lastHeartbeat).TotalMilliseconds;
		}

		/// <summary>
		/// Response to OP10 (HELLO) by heartbeating and identifying if necessary.
		/// </summary>
		/// <param name="payload"></param>
		private async Task OnHelloAsync(HelloPayload payload)
		{
			_heartbeatInterval = payload.HeartbeatInverval;
			_ = Task.Run(HeartbeatLoopAsync, _cancellation);

			if (string.IsNullOrEmpty(_sessionId))
				await IdentifyAsync().ConfigureAwait(false);
			else await ResumeAsync().ConfigureAwait(false);
		}
		
		/// <summary>
		/// Attempts to resume an existing session.
		/// </summary>
		private async Task ResumeAsync()
		{
			var gatewayResumePayload = new GatewayPayload<ResumePayload>
			{
				OpCode = GatewayOpCode.Resume,
				Data = new()
				{
					Token = StringUtils.GetFormattedToken(TokenType.Bot, _token),
					Sequence = _lastSequence,
					Session = _sessionId
				}
			};

			await _socketClient.SendMessageAsync(JsonConvert.SerializeObject(gatewayResumePayload));
			Console.WriteLine("Resumed Session.");
		}
		
		/// <summary>
		/// Identifies with the gateway to create a new session.
		/// </summary>
		private async Task IdentifyAsync()
		{
			var payload = new GatewayPayload<IdentifyPayload>
			{
				OpCode = GatewayOpCode.Identify,
				Data = new()
				{
					Token = _token,
					Intents = _intents,
					LargeThreshold = 250
				}
			};

			await _socketClient.SendMessageAsync(JsonConvert.SerializeObject(payload));
		}
	}
}