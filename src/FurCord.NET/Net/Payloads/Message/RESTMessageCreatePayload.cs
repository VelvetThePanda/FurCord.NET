using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FurCord.NET
{
	public struct RESTMessageCreatePayload
	{
		[JsonProperty("content")]
		public string Content { get; set; }
		
		[JsonProperty("tts")]
		public bool TTS { get; set; }

		[JsonProperty("message_reference")]
		public RESTMessageReference Reply { get; set; }
	}

	public struct RESTMessageReference
	{
		[JsonProperty("message_id")]
		public ulong MessageId { get; set; }

		[JsonProperty("fail_if_not_exists")]
		public bool FailIfNotExists => false;
	}
}