using System;
using System.Text.Json.Serialization;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// A Discord message.
	/// </summary>
	public interface IMessage
	{
		/// <summary>
		/// The content of this message, if any.
		/// </summary>
		[JsonPropertyName("content")]
		public string Content { get; }
		
		/// <summary>
		/// When this message was created.
		/// </summary>
		[JsonPropertyName("timestamp")]
		public DateTimeOffset CreationTimestamp { get; }
	}
}