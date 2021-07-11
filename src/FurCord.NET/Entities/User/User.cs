using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	
	public class User : IUser
	{
		[JsonProperty("id")]
		public ulong Id { get; internal set; }

		[JsonProperty("username")]
		public string Username { get; internal set; }
		
		[JsonProperty("avatar")]
		public string AvatarHash { get;  set; }
		
		[JsonProperty("discriminator")]
		public string Discriminator { get; internal set; }
		
		[JsonProperty("public_flags")]
		public int Flags { get; internal set; }

		[JsonProperty("bot")]
		public bool Bot { get; internal set; }
	}
}