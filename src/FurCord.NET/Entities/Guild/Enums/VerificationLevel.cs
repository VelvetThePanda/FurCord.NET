namespace FurCord.NET.Entities
{
	/// <summary>
	/// Requisite verification level held against server members.
	/// </summary>
	public enum VerificationLevel
	{
		/// <summary>
		/// No verification. Anyone can join and chat right away.
		/// </summary>
		None,

		/// <summary>
		/// Low verification level. Users are required to have a verified email attached to their account in order to be able to chat.
		/// </summary>
		Low,

		/// <summary>
		/// Medium verification level. Users are required to have a verified email attached to their account must've registered at least 5 minutes prior.
		/// </summary>
		Medium,

		/// <summary>
		/// (╯°□°）╯︵ ┻━┻ verification level. Users must have a verified email, an account older than 5 minutes, and must wait 10 minutes after joining the server before being allowed to chat.
		/// </summary>
		High,

		/// <summary>
		/// ┻━┻ ﾐヽ(ಠ益ಠ)ノ彡┻━┻ verification level. Requires a verified phone number.
		/// </summary>
		Highest
	}
}