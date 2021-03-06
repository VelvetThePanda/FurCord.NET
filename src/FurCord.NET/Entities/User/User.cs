using System;
using System.Threading.Tasks;
using FurCord.NET.Net;
using FurCord.NET.Utils;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	public sealed class User : IUser
	{
		public ulong Id { get; internal set; }

		IDiscordClient ISnowflake.Client { get; set; }
		
		public string Username { get; internal set; }

		public string AvatarUrl => !string.IsNullOrEmpty(AvatarHash) ? CDN.UserAvatar(Id, AvatarHash) : CDN.DefaultAvatar(Id, short.Parse(Discriminator));
		
		[JsonProperty("avatar")]
		public string AvatarHash { get; internal set; }
		
		public string Discriminator { get; internal set; }
		
		public int Flags { get; internal set; }
		
		public bool IsBot { get; internal set; }
		
		/// <summary>
		/// Sends a message to this user. 
		/// </summary>
		/// <param name="message">The message to send.</param>
		/// <returns>The returned message.</returns>
		public Task<IMessage> SendMessageAsync(string message) => (this as ISnowflake).Client.SendMessageAsync(this, message);
	}
}