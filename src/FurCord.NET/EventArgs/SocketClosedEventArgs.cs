using Emzi0767.Utilities;

namespace FurCord.NET
{
	/// <summary>
	/// Repesents that the gateway socket's connection has been terminated.
	/// </summary>
	public sealed class SocketClosedEventArgs : AsyncEventArgs
	{
		/// <summary>
		/// The response code for why this connection was terminated.
		/// </summary>
		public int CloseCode { get; internal set; }
		
		/// <summary>
		/// The resposne reason for why this connection was terminated.
		/// </summary>
		public string CloseMessage { get; internal set; }
		
		internal SocketClosedEventArgs() {}
	}
}