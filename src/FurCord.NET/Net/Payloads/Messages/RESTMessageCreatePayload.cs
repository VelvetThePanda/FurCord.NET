using System.Text.Json.Serialization;

namespace FurCord.NET
{
	public struct RESTMessageCreatePayload
	{
		[JsonPropertyName("content")]
		public string Content { get; set; }
		
		[JsonPropertyName("tts")]
		public bool TTS { get; set; }

		[JsonPropertyName("message_reference")]
		public RESTMessageReference Reply { get; set; }
	}

	public struct RESTMessageReference
	{
		[JsonPropertyName("message_id")]
		public ulong MessageId { get; set; }

		[JsonPropertyName("fail_if_not_exists")]
		public bool FailIfNotExists => false;
	}
}