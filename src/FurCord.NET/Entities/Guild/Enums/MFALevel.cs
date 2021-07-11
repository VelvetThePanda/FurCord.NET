namespace FurCord.NET.Entities
{
	/// <summary>
	/// Represents the multi-factor authenentication (mfa) level required by server members 
	/// </summary>
	public enum MFALevel
	{
		/// <summary>
		/// Server staff are <b>not</b> required to have MFA on their account to perform moderation actions.
		/// </summary>
		Disabled,
		/// <summary>
		/// Server staff <b>are</b> required to have MFW on their account to perform moderation actions.
		/// </summary>
		Enabled
	}
}