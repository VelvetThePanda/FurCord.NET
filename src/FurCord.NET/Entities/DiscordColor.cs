using System;

namespace FurCord.NET.Entities
{
	public readonly partial struct DiscordColor
	{
		private static readonly char[] HexAlphabet = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
		
		/// <summary>
		/// The value of this color, as an integer.
		/// </summary>
		public int Value { get; }

		/// <summary>
		/// The red component of this color.
		/// </summary>
		public byte RedComponent => (byte)((Value >> 16) & 0xFF);

		/// <summary>
		/// The green component of this color.
		/// </summary>
		public byte GreenComponent => (byte)((Value >> 8) & 0xFF);

		/// <summary>
		/// The blue component of this color.
		/// </summary>
		public byte BlueComponent => (byte)(Value & 0xFF);

		public DiscordColor(int color) => Value = color;
		
		public DiscordColor(float r, float g, float b)
		{
			if (r is < 0 or > 1 || g is < 0 or > 1 || b is < 0 or > 1)
				throw new InvalidOperationException( "Each component must be between 0.0 and 1.0 inclusive.");

			var redb	= (byte)(r * 255);
			var greenb	= (byte)(g * 255);
			var blueb	= (byte)(b * 255);

			this.Value = (redb << 16) | (greenb << 8) | blueb;
		}
		
		public override string ToString() => $"#{Value:X6}";
		
		public static implicit operator DiscordColor(int value) => new(value);
	}
}