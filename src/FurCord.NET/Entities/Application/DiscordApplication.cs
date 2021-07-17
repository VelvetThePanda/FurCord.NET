using FurCord.NET.Net;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// A Discord application.
	/// </summary>
	public sealed class DiscordApplication : ISnowflake
	{
		/// <summary>
		/// The Id of this application.
		/// </summary>
		[JsonProperty("id")]
		public ulong Id { get; internal set; }
		
		[JsonIgnore]
		IDiscordClient ISnowflake.Client { get; set; }

		/// <summary>
		/// The name of this application.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; internal set; }
		
		[JsonProperty("icon")]
		public string IconHash { get; internal set; }
		
		[JsonProperty("description")]
		public string Description { get; internal set; }
		
		[JsonProperty("rpc_origins")]
		public string[]? RpcRegions { get; internal set; }
		
		[JsonProperty("bot_public")]
		public bool IsPublic { get; internal set; }
		
		[JsonProperty("bot_require_code_grant")]
		public bool RequiresCodeGrant { get; internal set; }
		
		[JsonProperty("terms_of_service_url")]
		public string? TermsOfServiceUrl { get; internal set; }
		
		[JsonProperty("privacy_policy_url")]
		public string? PrivacyPolicyUrl { get; internal set; }
		
		[JsonProperty("owner")]
		public IUser? Owner { get; internal set; }
		
		[JsonProperty("summary")]
		public string Summary { get; internal set; }
		
		[JsonProperty("verify_key")]
		public string VerifyKey { get; internal set; }
		
		[JsonProperty("team")]
		public DiscordTeam? Team { get; internal set; }
		
		[JsonProperty("guild_id")]
		public ulong? GuildId { get; internal set; }
		
		[JsonProperty("slug")]
		public string SlugUrl { get; internal set; }
		
		[JsonProperty("cover_image")]
		public string CoverImageHash { get; internal set; }
		
		[JsonProperty("flags")]
		public ApplicationFlags Flags { get; internal set; }
	}
}