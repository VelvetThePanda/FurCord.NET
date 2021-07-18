namespace FurCord.NET.Utils
{
	/// <summary>
	/// A class containing non-parameterized routes to make REST requests to.
	/// </summary>
	public static class Routes
	{
		/// <summary>
		/// The route to create a DM.
		/// </summary>
		public const string CreateDM = "users/@me/channels";
		
		/// <summary>
		/// The route to create a message in a channel.
		/// </summary>
		public const string Messages = "channels/:channel_id/messages";
	}
}