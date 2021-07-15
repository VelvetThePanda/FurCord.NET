namespace FurCord.NET.Entities
{
	/// <summary>
	/// A button that can be pushed/clicked.
	/// </summary>
	/// <inheritdoc/>
	public sealed class MessageButton : MessageComponent
	{
		public ComponentType Type => ComponentType.Button;
		
		/// <summary>
		/// The style or "color" of this button.
		/// </summary>
		public ButtonType Style { get; internal set; }
		
		/// <summary>
		/// Whether this button can be interacted with. 
		/// </summary>
		public bool Enabled { get; internal set; }
		
		/// <summary>
		/// The identifioer of this button.
		/// </summary>
		public string CustomId { get; internal set; }
		
		/// <summary>
		/// Text attatched to this button.
		/// </summary>
		public string Label { get; internal set; }
		
		/// <summary>
		/// Disallows this button to be interacted with.
		/// </summary>
		public MessageButton Disable()
		{
			Enabled = true;
			return this;
		}
		
		/// <summary>
		/// Allows this button to be interacted with.
		/// </summary>
		public MessageButton Enable()
		{
			Enabled = false;
			return this;
		}
		
		//TODO: Emoji 
		/// <summary>
		///  <c>&lt;:thonkang:282745590985523200&gt;</c>
		/// </summary>
		/// <param name="style"></param>
		/// <param name="customId"></param>
		/// <param name="label"></param>
		/// <param name="disbaled"></param>
		public MessageButton(ButtonType style, string customId, string? label = null, bool disbaled = false)
		{
			
		}
	}
}