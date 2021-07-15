using Newtonsoft.Json;

namespace FurCord.NET.Net
{
	public sealed class GatewayInfo
	{
		[JsonProperty("url")]
		public string Url { get; internal set; }

		[JsonProperty("shards")]
		public long Shards { get; internal set; }

		[JsonProperty("session_start_limit")]
		public SessionStartLimit SessionStartLimit { get; internal set; }

		internal GatewayInfo() { }
	}

	public sealed class SessionStartLimit
	{
		[JsonProperty("total")]
		public long Total { get; internal set; }

		[JsonProperty("remaining")]
		public long Remaining { get; internal set; }

		[JsonProperty("reset_after")]
		public long ResetAfter { get; internal set; }

		[JsonProperty("max_concurrency")]
		public long MaxConcurrency { get; internal set; }

		internal SessionStartLimit() { }
	}
}