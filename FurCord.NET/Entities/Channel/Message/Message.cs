using System;

namespace FurCord.NET.Entities
{
	///<inheritdoc cref="IMessage"/>
	public class Message : Snowflake, IMessage
	{
		
		public string Content { get; internal set; }

		public DateTimeOffset CreationTimestamp => default;
	}
}