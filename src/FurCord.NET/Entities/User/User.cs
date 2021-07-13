using System;
using System.Threading.Tasks;
using FurCord.NET.Net;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	public sealed class User : IUser
	{
		public ulong Id { get; internal set; }

		IDiscordClient ISnowflake.Client { get; set; }
		
		public string Username { get; internal set; }

		public string AvatarUrl { get; internal set; }

		public string AvatarHash { get; internal set; }
		
		public string Discriminator { get; internal set; }
		
		public int Flags { get; internal set; }
		
		public bool IsBot { get; internal set; }
		
		/// <summary>
		/// Sends a message to this user. 
		/// </summary>
		/// <param name="message">The message to send.</param>
		/// <returns>The returned message.</returns>
		/// <exception cref="InvalidOperationException">The object's client was not set.</exception>
		public Task<IMessage> SendMessageAsync(IMessage message) 
			=> (this as ISnowflake).Client?.SendMessageAsync(this, message) ?? 
			   throw new InvalidOperationException("This object does not have a client associated with it. This is likely a cache error.");
	}
}