using Emzi0767.Utilities;

namespace FurCord.NET
{
	/// <summary>
	/// Represents that the client has been initialized and made a successful connection to the gateway.
	/// </summary>
	public sealed class ReadyEventArgs : AsyncEventArgs
	{
		internal ReadyEventArgs() { }
	}
}