using System;
using FurCord.NET.Entities.Converters;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// Represents a unique Discord object.
	/// </summary>
	//[JsonConverter(typeof(SnowflakeConverter))]
	public interface ISnowflake
	{
		/// <summary>
		/// The id of this snowflake.
		/// </summary>
		[JsonProperty("id")]
		public ulong Id { get; }
		
		/// <summary>
		/// Gets when this snowflake was created.
		/// </summary>
		[JsonProperty("timestamp")]
		public DateTimeOffset CreationDate => DiscordEpoch.AddSeconds(Id << 22);
		//TODO: Add client.

		private static readonly DateTimeOffset DiscordEpoch = new DateTime(2015, 1, 1);
	}
}