using Emzi0767.Utilities;

namespace FurCord.NET
{
	/// <summary>
	/// Repesents that the gateway socket's connection has been terminated.
	/// </summary>
	public sealed class SocketClosedEventArgs : AsyncEventArgs
	{
		public int CloseCode { get; internal set; }
		public string CloseMessage { get; internal set; }
		
		internal SocketClosedEventArgs() {}
	}
}