using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FurCord.NET.Entities.Converters;
using FurCord.NET.Net;
using FurCord.NET.Utils;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// Concrete implementation of <see cref="IMember"/>
	/// </summary>
	/// <inheritdoc cref="IMember"/>
	public sealed class Member : IMember
	{ 
		public ulong Id => User.Id;

		IDiscordClient ISnowflake.Client { get; set; }
		
		public string Username => User.Username;

		public string AvatarUrl => User.AvatarHash is not null ? 
			CDN.UserAvatar(Id, AvatarHash!) :
			CDN.DefaultAvatar(Id, short.Parse(Discriminator, NumberStyles.Number, CultureInfo.InvariantCulture));

		public string? AvatarHash => User.AvatarHash;

		public string Discriminator => User.Discriminator;
		public int Flags => User.Flags;
		public bool IsBot => User.IsBot;
		
		public Task<IMessage> SendMessageAsync(IMessage message)
			=> (this as ISnowflake).Client?.SendMessageAsync(this, message) ?? 
			   throw new InvalidOperationException("This object does not have a client associated with it. This is likely a cache error.");

		public string? Nickname { get; internal set; }

		public string GuildAvatarUrl => GuildAvatarHash is null ?  AvatarUrl  : CDN.GuildAvatar((this as IMember).GuildId, Id, GuildAvatarHash);
		
		public string? GuildAvatarHash { get; internal set; }
		
		ulong IMember.GuildId { get; set; }
		
		IGuild IMember.Guild { get; set; }
		
		IReadOnlyList<ulong> IMember.Roles { get; set; }

		[JsonProperty("user")]
		[JsonConverter(typeof(ConcreteTypeConverter<User>))]
		internal IUser User { get; set; }
	}
}