namespace FurCord.NET.Entities
{
	/// <summary>
	/// What type a component is.
	/// </summary>
	public enum ComponentType : short
	{
		/// <summary>
		/// A row of other components.
		/// </summary>
		ActionRow,
		
		/// <summary>
		/// A button that can be interacted with.
		/// </summary>
		Button,
		
		/// <summary>
		/// A selection menu that reveals several other options.
		/// </summary>
		Dropdown,
	}
}