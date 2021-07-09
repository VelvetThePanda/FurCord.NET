using System.Text.Json.Serialization;

namespace FurCord.NET
{
	internal struct RESTMessageCreatePayload
	{
		[JsonPropertyName("content")]
		public string Content { get; set; }
		
		[JsonPropertyName("tts")]
		public bool TTS { get; set; }
	}
}