using Newtonsoft.Json;

namespace FurCord.NET.Net.Payloads
{
	/// <summary>
	/// A payload used to create a Private Channel (DM)
	/// </summary>
	internal sealed record RESTCreateDMPayload
	{
		/// <summary>
		/// The Id of the recipient to create a DM channel with.
		/// </summary>
		[JsonProperty("recipient_id")]
		public ulong RecipientId { get; init; }

		public RESTCreateDMPayload() { }
		public RESTCreateDMPayload(ulong id) => RecipientId = id;
	}
}