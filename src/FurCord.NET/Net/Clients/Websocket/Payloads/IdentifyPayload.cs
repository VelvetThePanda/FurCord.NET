using System;
using FurCord.NET.Net.Enums;
using Newtonsoft.Json;

namespace FurCord.NET.Net
{
	internal sealed class IdentifyPayload : IGatewayPayloadData
	{
		[JsonProperty("token")]
		public string Token { get; set; }
		
		[JsonProperty("intents")]
		public GatewayIntents Intents { get; set; }

		[JsonProperty("compress")]
		public bool Compress => false; //Soon, maybe?//
		
		[JsonProperty("large_threshold")]
		public int LargeThreshold { get; set; }

		[JsonProperty("properties")]
		public ClientProperties Properties { get; } = new();

		internal sealed class ClientProperties
		{
			[JsonProperty("$os")]
			public string OS =>
				OperatingSystem.IsWindows() ? "windows" :
				OperatingSystem.IsMacOS() ? "osx" :
				OperatingSystem.IsLinux() ? "linux" : "unknown (raspi?)";

			[JsonProperty("$browser")]
			public string Browser => "FurCord";
			
			[JsonProperty("$device")]
			public string Device => Browser;
		}
	}
	
	internal sealed class ResumePayload : IGatewayPayloadData
	{
		[JsonProperty("token")]
		public string Token { get; set; }
		
		[JsonProperty("session_id")]
		public string Session { get; set; }
		
		[JsonProperty("seq")]
		public long Sequence { get; set; }
	}
}