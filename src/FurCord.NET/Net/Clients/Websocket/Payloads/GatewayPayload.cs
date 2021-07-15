using Newtonsoft.Json;

namespace FurCord.NET.Net
{
	internal sealed class GatewayPayload<T> where T : IGatewayPayloadData
	{
		[JsonProperty("op")]
		public GatewayOpCode OpCode { get; set; }
		
		[JsonProperty("d")]
		public T Data { get; set; }

		[JsonProperty("s", NullValueHandling = NullValueHandling.Ignore)]
		public int? Sequence { get; set; }
		
		[JsonProperty("t", NullValueHandling = NullValueHandling.Ignore)]
		public string EventName { get; set; }
	}
	
	internal sealed class GatewayPayload
	{
		[JsonProperty("op")]
		public GatewayOpCode OpCode { get; set; }
		
		[JsonProperty("d")]
		public object Data { get; set; }

		[JsonProperty("s")]
		public int? Sequence { get; set; }
		
		[JsonProperty("t")]
		public string EventName { get; set; }
	}
}