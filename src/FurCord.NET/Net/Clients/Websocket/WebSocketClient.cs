using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Emzi0767.Utilities;

namespace FurCord.NET.Net
{
	/// <summary>
	/// A concrete implementation of <see cref="IWebSocketClient"/>.
	/// </summary>
	/// <inheritdoc cref="IWebSocketClient"/>
	public sealed class WebSocketClient : IWebSocketClient
	{
		public bool IsConnected { get; private set; }
		
		// Headers to send with the request.
		public ConcurrentDictionary<string, string> Headers { get; } = new();

		public static IWebSocketClient CreateNew() => new WebSocketClient();
		
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
		private ClientWebSocket _webSocket = new();

		private readonly TimeSpan ExecutionTimeout = TimeSpan.FromSeconds(3);
		
		/// <summary>
		/// Connects to the specified uri, adding any headers if necessary.
		/// </summary>
		/// <param name="uri"></param>
		public async Task ConnectAsync(Uri uri)
		{
			if (IsConnected)
				await DisconnectAsync().ConfigureAwait(false);
			
			foreach ((var name, var value) in Headers)
				_webSocket.Options.SetRequestHeader(name, value);
			
			await _webSocket.ConnectAsync(uri, CancellationToken.None).ConfigureAwait(false);
			IsConnected = true;
			_ = ReceiveLoopAsync().ConfigureAwait(false);
		}
		
		/// <summary>
		/// Constructs a new <see cref="WebSocketClient"/>.
		/// </summary>
		internal WebSocketClient()
		{
			_cancellation = _cts.Token;
			_messageReceived = new("WS_MESSAGE_RX", ExecutionTimeout, EventErrorHandler);
			_socketErrored = new("SOCKET_ERRORED", ExecutionTimeout, EventErrorHandler);
			_socketClosed = new("SOCKET_CLOSED", ExecutionTimeout, EventErrorHandler);
		}
		
		/// <summary>
		/// Disconnects the client from the remote server.
		/// </summary>
		/// <param name="code">The remote server's response code.</param>
		/// <param name="message">The remote server's response message.</param>
		public async Task DisconnectAsync(int code = -1, string message = "")
		{
			await _sendingLock.WaitAsync().ConfigureAwait(false);

			if (_webSocket.State is WebSocketState.Open or WebSocketState.CloseReceived) 
				await _webSocket.CloseAsync((WebSocketCloseStatus) code, message, CancellationToken.None).ConfigureAwait(false);

			_cts.Cancel();
			_cts = new();
			_cancellation = _cts.Token;
			
			if (IsConnected)
			{
				IsConnected = false;
				_webSocket = new();
				await _socketClosed.InvokeAsync(this, new() {CloseCode = code, CloseMessage = message}).ConfigureAwait(false);
			}
			_sendingLock.Release();
		}
		
		public async Task SendMessageAsync(string message)
		{
			if (_webSocket?.State is not (WebSocketState.Open or WebSocketState.CloseReceived))
				return;
			try
			{
				await _sendingLock.WaitAsync(CancellationToken.None).ConfigureAwait(false);

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
			await using var bs = new MemoryStream();
			var pool = ArrayPool<byte>.Create();
			var buffer = pool.Rent(IncomingChunkSize);

			try
			{
				while (!_cancellation.IsCancellationRequested)
				{
					// See https://github.com/RogueException/Discord.Net/commit/ac389f5f6823e3a720aedd81b7805adbdd78b66d 
					// for explanation on the cancellation token

					WebSocketReceiveResult result;
					do
					{
						result = await _webSocket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);

						if (result.MessageType is WebSocketMessageType.Close)
							break;

						await bs.WriteAsync(buffer, 0, result.Count, CancellationToken.None).ConfigureAwait(false);
					}
					while (!result.EndOfMessage);

					var streamLength = (int) bs.Length;
					var sBuffer = ArrayPool<byte>.Shared.Rent(streamLength);
					bs.Position = 0;
					await bs.ReadAsync(sBuffer, 0, streamLength, CancellationToken.None).ConfigureAwait(false);
					
					switch (result.MessageType)
					{
						case WebSocketMessageType.Text:
							await _messageReceived.InvokeAsync(this, new(sBuffer[..streamLength]));
							ArrayPool<byte>.Shared.Return(sBuffer, true);
							bs.SetLength(0);
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
				}
			}
			catch (Exception exception)
			{
				await _socketErrored.InvokeAsync(this, new() {Exception = exception});
			}
			finally
			{
				//ArrayPool<byte>.Shared.Return(buffer, true);
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
			
			Console.WriteLine($"An exception was thrown from the invocation of an asyncrhonous event handler. {ex}");
			//TODO: Log
		}
	}
}