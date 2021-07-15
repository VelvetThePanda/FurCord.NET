using System.Text;
using Emzi0767.Utilities;

namespace FurCord.NET
{
	/// <summary>
	/// Represents that a message has been received over the gateway.
	/// </summary>
	public sealed class SocketMessageEventArgs : AsyncEventArgs
	{
		/// <summary>
		/// The message received.
		/// </summary>
		public string Message { get; }

		/// <summary>
		/// Constructs a new <see cref="SocketMessageEventArgs"/> by converting the received bytes into a readable string.
		/// </summary>
		/// <param name="message"></param>
		internal SocketMessageEventArgs(byte[] message) => Message = Encoding.UTF8.GetString(message);
	}
}