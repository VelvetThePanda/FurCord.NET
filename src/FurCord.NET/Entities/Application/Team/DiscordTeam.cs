using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FurCord.NET.Net;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// A Team
	/// </summary>
	public sealed class DiscordTeam : ISnowflake
	{
		/// <summary>
		/// The Id of this team.
		/// </summary>
		public ulong Id { get; set; }

		/// <summary>
		/// The members of this team.
		/// </summary>
		public IReadOnlyDictionary<ulong, DiscordTeamMember> TeamMembers => _teamMembers;

		[JsonProperty("members")]
		[JsonConverter(typeof(SnowflakeDictionaryConverter<DiscordTeamMember>))]
		private ConcurrentDictionary<ulong, DiscordTeamMember> _teamMembers;

		IDiscordClient ISnowflake.Client { get; set; }
		
		internal DiscordTeam() { }
	}
}