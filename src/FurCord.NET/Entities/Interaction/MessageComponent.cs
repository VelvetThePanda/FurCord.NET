using System;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	/// <summary>
	/// An interactable component attatched to a message.
	/// </summary>
	public abstract class MessageComponent
	{
		/// <summary>
		/// The type of component.
		/// </summary>
		[JsonProperty("type")]
		public ComponentType  Type { get; internal set; }
		
		/// <summary>
		/// Child components. Only applicable if <see cref="Type"/> is <see cref="ComponentType.ActionRow"/>.
		/// </summary>
		[JsonProperty("components")]
		public virtual MessageComponent[]? Components
		{
			get => throw new NotSupportedException();
			internal set => throw new NotSupportedException(); // In regards to these throwing, only ActionRow supports components, so no point. //
		}
	}

	/// <summary>
	/// A default class to used as a placeholder for new, unsupported component types.
	/// </summary>
	internal class DefaultMessageComponent : MessageComponent { }
}