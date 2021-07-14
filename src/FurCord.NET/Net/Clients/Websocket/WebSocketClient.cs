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
using FurCord.NET.EventArgs;

namespace FurCord.NET.Net.Websocket
{
	public sealed class WebSocketClient : IWebsocketClient
	{
		private const int OutgoingChunkSize = 4096; // 4 KiB
		private const int IncomingChunkSize = 32768; // 32 KiB
		
		private readonly SemaphoreSlim _connectionLock = new(1);
		private readonly SemaphoreSlim _sendingLock = new(1);
		private readonly CancellationToken _cancellation = new();
		private readonly ClientWebSocket _underlyingSocket = new();
		
		public ConcurrentDictionary<string, string> Headers { get; } = new();
		public async Task ConnectAsync(Uri uri)
		{
			try { await DisconnectAsync().ConfigureAwait(false); }
			catch { }
			
			await _connectionLock.WaitAsync(_cancellation).ConfigureAwait(false);
			await _underlyingSocket.ConnectAsync(uri, _cancellation).ConfigureAwait(false);
			_ = ReceiveLoopAsync();
			_connectionLock.Release();
		}

		public WebSocketClient(CancellationToken token = default)
		{
			_cancellation = token == default ? _cancellation : token;
			_messageReceived = new("WS_MESSAGE_RX", TimeSpan.FromSeconds(1), EventErrorHandler);
		}
		
		public async Task DisconnectAsync(int code = -1, string message = "") { }
		public async Task SendMessageAsync(string message)
		{
			if (_underlyingSocket?.State is not (WebSocketState.Open or WebSocketState.CloseReceived))
				return;
			try
			{
				await _sendingLock.WaitAsync(_cancellation).ConfigureAwait(false);

				var bytes = Encoding.UTF8.GetBytes(message);
				var byteLength = bytes.Length;

				var chunks = byteLength / OutgoingChunkSize is var outchunk and 0 ? outchunk : byteLength / OutgoingChunkSize + 1;

				for (int i = 0; i < chunks; i++)
				{
					var start = OutgoingChunkSize * i;
					var length = Math.Min(OutgoingChunkSize, byteLength - start);

					await _underlyingSocket
						.SendAsync(new(bytes, start, length), WebSocketMessageType.Text, i == byteLength - 1, _cancellation)
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
			await Task.Yield();
			var buffer = ArrayPool<byte>.Shared.Rent(IncomingChunkSize);

			try
			{
				do
				{
					WebSocketReceiveResult result;
					do
					{
						result = await _underlyingSocket.ReceiveAsync(buffer, CancellationToken.None);
						
						if (result.MessageType is WebSocketMessageType.Close)
							break;
						
					} while (!result.EndOfMessage);

					switch (result.MessageType)
					{
						case WebSocketMessageType.Text:
							await _messageReceived.InvokeAsync(this, new(buffer));
							break;
						
						case WebSocketMessageType.Close:
							break;

						case WebSocketMessageType.Binary:
						default:
							throw new NotSupportedException();
					}
					

				} while (!_cancellation.IsCancellationRequested);
			}
			catch (Exception exception) { /* Do something here. */}
		
		}

		public event AsyncEventHandler<IWebsocketClient, SocketMessageEventArgs>? MessageReceived
		{
			add => _messageReceived.Register(value);
			remove => _messageReceived.Unregister(value);
		}

		private readonly  AsyncEvent<WebSocketClient, SocketMessageEventArgs> _messageReceived;


		private void EventErrorHandler<TArgs>(
			AsyncEvent<WebSocketClient, TArgs> asyncEvent, Exception ex,
			AsyncEventHandler<WebSocketClient, TArgs> handler, WebSocketClient sender, TArgs eventArgs) 
			where TArgs : AsyncEventArgs 
		{ }
	}
}