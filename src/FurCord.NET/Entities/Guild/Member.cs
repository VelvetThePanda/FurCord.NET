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
		public ulong Id => (this as IMember).User.Id;

		IDiscordClient ISnowflake.Client { get; set; }
		
		public string Username => (this as IMember).User.Username;

		public string AvatarUrl => (this as IMember).User.AvatarHash is not null ? 
			CDN.UserAvatar(Id, AvatarHash!) :
			CDN.DefaultAvatar(Id, short.Parse(Discriminator, NumberStyles.Number, CultureInfo.InvariantCulture));

		public string? AvatarHash => (this as IMember).User.AvatarHash;

		public string Discriminator => (this as IMember).User.Discriminator;
		public int Flags => (this as IMember).User.Flags;
		public bool IsBot => (this as IMember).User.IsBot;
		
		public string? Nickname { get; internal set; }

		public string GuildAvatarUrl => GuildAvatarHash is null ?  AvatarUrl  : CDN.GuildAvatar((this as IMember).GuildId, Id, GuildAvatarHash);
		
		public string? GuildAvatarHash { get; internal set; }
		
		ulong IMember.GuildId { get; set; }
		
		IGuild IMember.Guild { get; set; }
		
		IReadOnlyList<ulong> IMember.Roles { get; set; }

		public Task<IMessage> SendMessageAsync(string content) => (this as IMember).User.SendMessageAsync(content);
		
		[JsonProperty("user")]
		[JsonConverter(typeof(ConcreteTypeConverter<User>))]
		IUser IMember.User { get; set; }
	}
}