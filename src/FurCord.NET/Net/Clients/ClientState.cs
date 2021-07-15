namespace FurCord.NET.Net
{
	/// <summary>
	/// Represents the current state of a client's connection.
	/// </summary>
	public enum ClientState
	{
		/// <summary>
		/// The client either has not started yet, or has been disconnected.
		/// </summary>
		Disconnected,
		/// <summary>
		/// The client has been connected.
		/// This state is indicative of immediately after opening a websocket connection and after caching.
		/// </summary>
		Connected,
		/// <summary>
		/// <para>Heartbeats are not being acked and the client's connection is considered zombied.</para>
		///
		/// <para>This could be an issue with the remote server.</para>
		/// </summary>
		Zombied,
		/// <summary>
		/// The client is in the process of caching data.
		/// </summary>
		Caching,
	}
}