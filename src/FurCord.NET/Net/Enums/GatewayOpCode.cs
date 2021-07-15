namespace FurCord.NET.Net
{
	internal enum GatewayOpCode
	{
		/// <summary>
		/// Used for dispatching events.
		/// </summary>
		Dispatch = 0,

		/// <summary>
		/// Sent to or from the gateway.
		/// <br/>
		///	<b>When sending</b>: Used to tell the gateway the client is still connected.
		/// <b>When receiving</b>: Used to tell the client to immediately send a heartbeat.
		/// </summary>
		Heartbeat = 1,

		/// <summary>
		/// Used for initial handshake with the gateway.
		/// </summary>
		Identify = 2,

		/// <summary>
		/// Used to update client status.
		/// </summary>
		StatusUpdate = 3,

		/// <summary>
		/// Used to update voice state, when joining, leaving, or moving between voice channels.
		/// </summary>
		VoiceStateUpdate = 4,

		/// <summary>
		/// Sent to the voice gateway to keep a session alive.
		/// </summary>
		VoiceServerPing = 5,

		/// <summary>
		/// Sent to the gateway to resume an existing session.
		/// </summary>
		Resume = 6,

		/// <summary>
		/// Sent by the gateway to tell the client to reconnect.
		/// </summary>
		Reconnect = 7,

		/// <summary>
		/// Sent to the gateway to request guild members.
		/// </summary>
		RequestGuildMembers = 8,

		/// <summary>
		/// Sent by the gateway that a session has been invalidated.
		/// </summary>
		InvalidSession = 9,

		/// <summary>
		/// Sent by the gateway to signify a websocket connection has been established.
		/// </summary>
		Hello = 10,

		/// <summary>
		/// Sent from the gateway to tell the client that the gateway has acknowledged the sent heartbeat.
		/// </summary>
		HeartbeatAck = 11,

		/// <summary>
		/// 
		/// </summary>
		GuildSync = 12
	}
}