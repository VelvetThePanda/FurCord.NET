using System;
using System.Threading.Tasks;
using FurCord.NET;
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

		private int _skippedHeartbeats;

			
		private async Task HandleDispatch(IWebSocketClient client, SocketMessageEventArgs args)
		{
			var payload = JsonConvert.DeserializeObject<GatewayPayload>(args.Message)!;

			var dispatchTask = payload.OpCode switch
			{
				GatewayOpCode.Dispatch => Task.CompletedTask,
				GatewayOpCode.Reconnect => throw new NotSupportedException(),
				GatewayOpCode.Hello => OnHelloAsync((payload.Data as JObject)!.ToObject<HelloPayload>()!)
			};
			_lastSequence = payload.Sequence ?? _lastSequence;
			await dispatchTask;
		}

		private async Task OnHelloAsync(HelloPayload payload)
		{
			_heartbeatInterval = payload.HeartbeatInverval;
			_ = Task.Run(HeartbeatLoopAsync, _cancellation);
			
			if (string.IsNullOrEmpty(_sessionId))
				await IdentifyAsync();
			//TODO: Resume
		}

		private async Task Resume()
		{
			
		}
		
		
		private async Task HeartbeatLoopAsync()
		{
			while (!_cancellation.IsCancellationRequested)
			{
				try
				{
					_skippedHeartbeats++;
					
					var heartbeat = new GatewayPayload
					{
						OpCode = GatewayOpCode.Heartbeat,
						Data = _lastSequence
					};

					await SocketClient.SendMessageAsync(JsonConvert.SerializeObject(heartbeat));
					await Task.Delay(_heartbeatInterval, _cancellation);
				}
				catch (TaskCanceledException) { }
			}
		}

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

			await SocketClient.SendMessageAsync(JsonConvert.SerializeObject(payload));
		}
	}
}