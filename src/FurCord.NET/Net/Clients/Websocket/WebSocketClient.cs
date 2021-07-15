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

namespace FurCord.NET.Net
{
	public sealed class WebSocketClient : IWebSocketClient
	{
		private const int OutgoingChunkSize = 4096; // 4 KiB
		private const int IncomingChunkSize = 32768; // 32 KiB
		
		private readonly SemaphoreSlim _connectionLock = new(1);
		private readonly SemaphoreSlim _sendingLock = new(1);
		private readonly CancellationToken _cancellation;
		private readonly ClientWebSocket _underlyingSocket = new();
		
		public ConcurrentDictionary<string, string> Headers { get; } = new();

		public static IWebSocketClient CreateNew() => new WebSocketClient();
		
		public async Task ConnectAsync(Uri uri)
		{
			try { await DisconnectAsync().ConfigureAwait(false); }
			catch { }
			
			foreach ((var name, var value) in Headers)
				_underlyingSocket.Options.SetRequestHeader(name, value);
			
			await _connectionLock.WaitAsync(_cancellation).ConfigureAwait(false);
			
			await _underlyingSocket.ConnectAsync(uri, _cancellation).ConfigureAwait(false);
			
			
			_ = Task.Run(ReceiveLoopAsync);
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

				var chunks = byteLength / OutgoingChunkSize is 0 ? 1 : byteLength / OutgoingChunkSize + 1;

				for (int i = 0; i < chunks; i++)
				{
					var start = OutgoingChunkSize * i;
					var length = Math.Min(OutgoingChunkSize, byteLength - start);

					await _underlyingSocket
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
			var buffer = ArrayPool<byte>.Shared.Rent(IncomingChunkSize);

			try
			{
				do
				{
					WebSocketReceiveResult result;
					do
					{
						result = await _underlyingSocket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
						
						if (result.MessageType is WebSocketMessageType.Close)
							break;
					} while (!result.EndOfMessage);
					
					switch (result.MessageType)
					{
						case WebSocketMessageType.Text:
							await _messageReceived.InvokeAsync(this, new(buffer[..result.Count]));
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

		public event AsyncEventHandler<IWebSocketClient, SocketMessageEventArgs>? MessageReceived
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