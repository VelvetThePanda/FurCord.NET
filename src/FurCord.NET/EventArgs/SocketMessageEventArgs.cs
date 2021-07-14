using System.Text;
using Emzi0767.Utilities;

namespace FurCord.NET.EventArgs
{
	public sealed class SocketMessageEventArgs : AsyncEventArgs
	{
		public string Message { get; }

		internal SocketMessageEventArgs(string message) => Message = message;

		internal SocketMessageEventArgs(byte[] message) => Message = Encoding.UTF8.GetString(message);
	}
}