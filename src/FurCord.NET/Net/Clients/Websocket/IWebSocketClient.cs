using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Emzi0767.Utilities;

namespace FurCord.NET.Net
{
	public delegate IWebSocketClient WebSocketClientFactoryDelegate();
	
	public interface IWebSocketClient
	{
		/// <summary>
		/// Gets whether this client has an open connection to the remote server.
		/// </summary>
		public bool IsConnected { get; }
		
		public ConcurrentDictionary<string, string> Headers { get; }
		
		/// <summary>
		/// Establishes a connection to the specified remote WebSocket endpoint.
		/// </summary>
		/// <param name="uri">The endpoint to connect to.</param>
		public Task ConnectAsync(Uri uri);
		
		/// <summary>
		/// Disconnects the WebSocket connection.
		/// </summary>
		/// <param name="code">The response code from the remote server.</param>
		/// <param name="message">Why the connection was closed.</param>
		public Task DisconnectAsync(int code = -1, string message = "");
		
		/// <summary>
		/// Sends a message to the specified websocket server.
		/// </summary>
		/// <param name="message">The message to send.</param>
		public Task SendMessageAsync(string message);
		
		/// <summary>
		/// An event that fires when a message is received from the remote server.
		/// </summary>
		public event AsyncEventHandler<IWebSocketClient, SocketMessageEventArgs> MessageReceived;

		/// <summary>
		/// An event that fires when the websocket supresses an exception.
		/// </summary>
		public event AsyncEventHandler<IWebSocketClient, SocketErroredEventArgs> SocketErorred;

		/// <summary>
		/// An event that fires when the socket connection is closed. 
		/// </summary>
		public event AsyncEventHandler<IWebSocketClient, SocketClosedEventArgs> SocketClosed;
		
	}
}