using Newtonsoft.Json;

namespace FurCord.NET.Net
{
	/// <summary>
	/// Hello (OP10) Payload. Sent down by the gateway after sending OP2 (Identify).
	/// </summary>
	internal sealed class HelloPayload
	{
		/// <summary>
		/// How often (in milliseconds) the client should heartbeat.
		/// </summary>
		[JsonProperty("heartbeat_interval")]
		public int HeartbeatInverval { get; private set; }
		
		
	}
}