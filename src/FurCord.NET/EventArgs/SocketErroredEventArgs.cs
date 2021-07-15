using System;
using Emzi0767.Utilities;

namespace FurCord.NET
{
	/// <summary>
	/// Represents that an exception has been thrown from the gateway socket.
	/// </summary>
	public sealed class SocketErroredEventArgs : AsyncEventArgs
	{
		/// <summary>
		/// Gets the exception thrown by the websocket client.
		/// </summary>
		public Exception Exception { get; internal set; }

		internal SocketErroredEventArgs() { }
	}
}