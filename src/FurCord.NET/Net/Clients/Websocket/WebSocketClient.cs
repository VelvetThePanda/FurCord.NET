using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Emzi0767.Types;
using Emzi0767.Utilities;
using FurCord.NET;

namespace FurCord.NET.Net
{
	public sealed class WebSocketClient : IWebSocketClient
	{
		//Max payload size is 4096 bytes.
		private const int OutgoingChunkSize = 4096; // 4 KiB
		//I don't know why we set it to 32KiB.
		private const int IncomingChunkSize = 32768; // 32 KiB
		
		//Prevents simultaneous sending of messages.
		private readonly SemaphoreSlim _sendingLock = new(1);
		
		// Used to stop the receive loop. //
		private CancellationTokenSource _cts = new();
		private CancellationToken _cancellation;
		
		// The actual socket that makes this work.
		private readonly ClientWebSocket _webSocket = new();
		
		// Headers to send with the request.
		public ConcurrentDictionary<string, string> Headers { get; } = new();

		public static IWebSocketClient CreateNew() => new WebSocketClient();
		
		/// <summary>
		/// Connects to the specified uri, adding any headers if necessary.
		/// </summary>
		/// <param name="uri"></param>
		public async Task ConnectAsync(Uri uri)
		{
			try { await DisconnectAsync().ConfigureAwait(false); }
			catch { }

			_cts = new();
			_cancellation = _cts.Token;
			
			foreach ((var name, var value) in Headers)
				_webSocket.Options.SetRequestHeader(name, value);
			
			await _webSocket.ConnectAsync(uri, _cancellation).ConfigureAwait(false);

			_ = ReceiveLoopAsync().ConfigureAwait(false);
		}

		internal WebSocketClient()
		{
			_cancellation = _cts.Token;
			_messageReceived = new("WS_MESSAGE_RX", TimeSpan.FromSeconds(1), EventErrorHandler);
			_socketErrored = new("SOCKET_ERRORED", TimeSpan.FromSeconds(1), EventErrorHandler);
		}

		public async Task DisconnectAsync(int code = -1, string message = "")
		{
			_cts.Cancel();
		}
		
		
		public async Task SendMessageAsync(string message)
		{
			if (_webSocket?.State is not (WebSocketState.Open or WebSocketState.CloseReceived))
				return;
			try
			{
				await _sendingLock.WaitAsync(_cancellation).ConfigureAwait(false);

				var bytes = Encoding.UTF8.GetBytes(message);
				var byteLength = bytes.Length;

				var chunks = byteLength / OutgoingChunkSize is 0 ? 1 : byteLength / OutgoingChunkSize + 1;

				for (int i = 0; i < chunks; i++)
				{
					var start = OutgoingChunkSize * i;
					var length = Math.Min(OutgoingChunkSize, byteLength - start);

					await _webSocket
						.SendAsync(new(bytes, start, length), WebSocketMessageType.Text, i == chunks - 1, _cancellation)
						.ConfigureAwait(false);
				}
			}
			catch (OperationCanceledException) { }
			finally
			{
				_sendingLock.Release();
			}
		}
		
		internal async Task ReceiveLoopAsync()
		{
			byte[] buffer = ArrayPool<byte>.Shared.Rent(IncomingChunkSize);

			try
			{
				do
				{
					WebSocketReceiveResult result;
					do
					{
						result = await _webSocket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);

						if (result.MessageType is WebSocketMessageType.Close)
							break;
					} while (!result.EndOfMessage);

					switch (result.MessageType)
					{
						case WebSocketMessageType.Text:
							await _messageReceived.InvokeAsync(this, new(buffer[..result.Count]));
							break;

						case WebSocketMessageType.Close:
							await _webSocket.CloseOutputAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None).ConfigureAwait(false);
							await _socketClosed.InvokeAsync(this, new() {CloseCode = (int) result.CloseStatus, CloseMessage = result.CloseStatusDescription!});
							break;

						case WebSocketMessageType.Binary:
							throw new NotSupportedException();
						default:
							throw new ArgumentOutOfRangeException();
					}

				} while (!_cancellation.IsCancellationRequested);
			}
			catch (Exception exception)
			{
				await _socketErrored.InvokeAsync(this, new() {Exception = exception});
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(buffer, true);
			}
			
		}

		public event AsyncEventHandler<IWebSocketClient, SocketMessageEventArgs>? MessageReceived
		{
			add => _messageReceived.Register(value);
			remove => _messageReceived.Unregister(value);
		}
		
		private readonly AsyncEvent<WebSocketClient, SocketMessageEventArgs> _messageReceived;

		public event AsyncEventHandler<IWebSocketClient, SocketErroredEventArgs>? SocketErorred
		{
			add => _socketErrored.Register(value);
			remove => _socketErrored.Unregister(value);
		}

		private readonly AsyncEvent<WebSocketClient, SocketErroredEventArgs> _socketErrored;

		public event AsyncEventHandler<IWebSocketClient, SocketClosedEventArgs>? SocketClosed
		{
			add => _socketClosed.Register(value);
			remove => _socketClosed.Unregister(value);
		}

		private readonly AsyncEvent<WebSocketClient, SocketClosedEventArgs> _socketClosed;
		
		private void EventErrorHandler<TArgs>(
			AsyncEvent<WebSocketClient, TArgs> asyncEvent, Exception ex,
			AsyncEventHandler<WebSocketClient, TArgs> handler, WebSocketClient sender, TArgs eventArgs)
			where TArgs : AsyncEventArgs
		{
			
		}
	}
}