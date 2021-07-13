using System;
using System.Threading.Tasks;
using FurCord.NET.Entities.Converters;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// A Discord user.
	/// </summary>
	public interface IUser : ISnowflake
	{
		/// <summary>
		/// The username of this user.
		/// </summary>
		[JsonProperty("username")]
		public string Username { get; }
		
		/// <summary>
		/// The image url of this user's avatar.
		/// </summary>
		[JsonIgnore]
		public string AvatarUrl { get; }
		
		/// <summary>
		/// The hash of this user's avatar.
		/// </summary>
		[JsonProperty("avatar")]
		public string? AvatarHash { get; }
		
		/// <summary>
		/// This user's discriminator.
		/// </summary>
		[JsonProperty("discriminator")]
		public string Discriminator { get; }
		
		/// <summary>
		/// The 
		/// </summary>
		[JsonProperty("public_flags")]
		public int Flags { get; }

		/// <summary>
		/// Whether this user is a bot.
		/// </summary>
		[JsonProperty("bot")]
		public bool IsBot { get; }

		/// <summary>
		/// Sends a message to this user. 
		/// </summary>
		/// <param name="message">The message to send.</param>
		/// <returns>The returned message.</returns>
		/// <exception cref="InvalidOperationException">The object's client was not set.</exception>
		public Task<IMessage> SendMessageAsync(IMessage message);
	}
}