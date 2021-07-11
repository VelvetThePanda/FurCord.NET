namespace FurCord.NET.Entities
{
	/// <summary>
	/// The default notification setting for a server.
	/// </summary>
	public enum DefaultMessagenotifications
	{
		/// <summary>
		/// All messages will send a notification. 
		/// </summary>
		AllMessages,
		/// <summary>
		/// Only mentions for a given user will send a notication.
		/// </summary>
		MentionsOnly
	}
}