using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurCord.NET.Entities;
using FurCord.NET.Net.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FurCord.NET.Net
{
	internal partial class DiscordClient
	{
		private long? _lastSequence;
		private string? _sessionId;
		private int _heartbeatInterval;

		private DateTime _lastHeartbeat;

		private int _skippedHeartbeats;
		private bool _guildDownloadComplete;

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

		private async Task SendGatewayMessageAsync(string message)
		{
			_logger.LogTrace("[Gateway ↑] {Payload}", message);
			await _socketClient.SendMessageAsync(message);
		}
		
		#region Gateway Dispatch

		private async Task HandleDispatchAsync(IWebSocketClient client, SocketMessageEventArgs args)
		{
			_logger.LogTrace("[Gateway ↓] {Payload}", args.Message);
			var payload = JsonConvert.DeserializeObject<GatewayPayload>(args.Message)!;
			
			_lastSequence = payload.Sequence ?? _lastSequence;
			var dispatchTask = payload.OpCode switch
			{
				GatewayOpCode.Dispatch => HandleGatewayDispatchAsync(payload),
				GatewayOpCode.Reconnect => OnReconnectAsync(),
				GatewayOpCode.Hello => OnHelloAsync((payload.Data as JObject)!.ToObject<HelloPayload>()!),
				GatewayOpCode.InvalidSession => OnInvalidSessionAsync((bool)payload.Data),
				GatewayOpCode.HeartbeatAck => AckHeartbeatAsync(),
				_ => Task.CompletedTask
			};
			await dispatchTask.ConfigureAwait(false);
		}

		private async Task OnReconnectAsync()
		{
			_logger.LogDebug("Received RECONNECT (OP7). Reconnecting.");
			await _socketClient.DisconnectAsync(-1, "Acknowledged OP7");
		}
		
		private async Task OnInvalidSessionAsync(bool resumable)
		{
			_logger.LogDebug("Received INVALID SESSION (OP9, Resumable: {Resumable})", resumable);

			if (!resumable)
			{
				_sessionId = null;
				await IdentifyAsync().ConfigureAwait(false);
			}
			else
			{
				await Task.Delay(3000).ConfigureAwait(false);
				await ResumeAsync().ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Acknowledges  
		/// </summary>
		private async Task AckHeartbeatAsync()
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
			_logger.LogDebug("Starting heartbeat task.");
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
					Token = Utils.Utils.GetFormattedToken(TokenType.Bot, _token),
					Sequence = _lastSequence ?? 0,
					Session = _sessionId
				}
			};

			await SendGatewayMessageAsync(JsonConvert.SerializeObject(gatewayResumePayload)).ConfigureAwait(false);
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

			await SendGatewayMessageAsync(JsonConvert.SerializeObject(payload)).ConfigureAwait(false);
		}
		
		#endregion
		
		private async Task HandleGatewayDispatchAsync(GatewayPayload payload)
		{
			if (payload.Data is not JObject job)
			{
				_logger.LogDebug("Payload data was not of expected type. This is probably safe to ignore. Payload: {Payload}", payload.Data);
				return;
			}

			switch (payload.EventName)
			{
				case "READY":
					_sessionId = job["session_id"]!.ToString();
					var guilds = job["guilds"]!.ToObject<IEnumerable<Guild>>()!;
					_guildDownloadComplete = false;
					foreach (var g in guilds)
						_guilds[g.Id] = g;
					break;
				
				case "GUILD_CREATE":
					var id = (ulong) job["id"]!;
					var cachedGuild = _guilds[id] = job.ToObject<Guild>()!;
					cachedGuild.Client = this;
					cachedGuild.PopulateObjects();

					var oldDownloadStatus = _guildDownloadComplete;
					_guildDownloadComplete = _guilds.All(g => !g.Value.Unavailable);
					
					//if (!oldDownloadComplete && _guildDownloadComplete) await 
					
					break;
				
				case "RESUME":
					_logger.LogDebug("Resumed session.");
					break;
			}
			
		}
	}
}