namespace FurCord.NET.Entities
{
	public readonly partial struct DiscordColor
	{
		#region Noirchrome

		/// <summary>
		/// No color.
		/// </summary>
		public static DiscordColor None { get; } = new(0);

		/// <summary>
		/// As close to black as Discord allows.
		/// </summary>
		public static DiscordColor Black { get; } = new(0x010101);

		/// <summary>
		/// Pure white.
		/// </summary>
		public static DiscordColor White { get; } = new(0xFFFFFF);

		/// <summary>
		/// A neutral gray.
		/// </summary>
		public static DiscordColor Grey { get; } = new(0x808080);
		
		/// <summary>
		/// Darker grey.
		/// </summary>
		public static DiscordColor DarkGray { get; } = new(0xA9A9A9);
		
		/// <summary>
		/// Lighter grey.
		/// </summary>
		public static DiscordColor LightGray { get; } = new(0xD3D3D3);
		
		/// <summary>
		/// Grey, but much darker.
		/// </summary>
		public static DiscordColor VeryDarkGray { get; } = new(0x666666);

		#endregion

		#region Old branding colors

		/// <summary>
		/// Discord's old blurple color.
		/// </summary>
		public static DiscordColor OldBlurple { get; } = new(0x7289DA);
		
		/// <summary>
		/// Discord's old grey color.
		/// </summary>
		public static DiscordColor Grayple { get; } = new(0x99AAB5);
		
		/// <summary>
		/// Discord's old dark color.
		/// </summary>
		public static DiscordColor Dark { get; } = new(0x2C2F33);
		
		/// <summary>
		/// Discord's old color, which is almost a very dark grey.
		/// </summary>
		public static DiscordColor NotQuiteBlack { get; } = new(0x23272A);
		
		#endregion

		#region New branding colors

		/// <summary>
		/// Discord's new blurple.
		/// </summary>
		public static DiscordColor Blurple { get; } = new(0x5865F2);
		
		/// <summary>
		/// Discord's green. 
		/// </summary>
		public static DiscordColor DiscordGreen { get; } = new(0x57F287);
		
		/// <summary>
		/// Discord's yellow.
		/// </summary>
		public static DiscordColor DiscordYellow { get; } = new(0xFEE75C);

		/// <summary>
		/// Discord's pink.
		/// </summary>
		public static DiscordColor Fuchsia { get; } = new(0xEB459E);

		/// <summary>
		/// Discord's red.
		/// </summary>
		public static DiscordColor DiscordRed { get; } = new(0xED4245);
		
		#endregion

		#region Remaining Colors
		
		/// <summary>
		/// Pure red.
		/// </summary>
        public static DiscordColor Red { get; } = new(0xFF0000);
		
        public static DiscordColor DarkRed { get; } = new(0x7F0000);

		/// <summary>
		/// Pure green.
		/// </summary>
        public static DiscordColor Green { get; } = new(0x00FF00);

        /// <summary>
        /// Darker green.
        /// </summary>
        public static DiscordColor DarkGreen { get; } = new(0x007F00);

        /// <summary>
        /// Pure blue.
        /// </summary>
        public static DiscordColor Blue { get; } = new(0x0000FF);

        /// <summary>
        /// Darker blue.
        /// </summary>
        public static DiscordColor DarkBlue { get; } = new(0x00007F);

		/// <summary>
		/// Pure yellow.
		/// </summary>
        public static DiscordColor Yellow { get; } = new(0xFFFF00);
        
        public static DiscordColor Cyan { get; } = new(0x00FFFF);
        
        public static DiscordColor Magenta { get; } = new(0xFF00FF);
        
        public static DiscordColor Teal { get; } = new(0x008080);
        
        public static DiscordColor Aquamarine { get; } = new(0x00FFBF);
        
        public static DiscordColor Gold { get; } = new(0xFFD700);
        
        /// <summary>
        /// Goldenrod. An orange-y yellowy color.
        /// </summary>
        public static DiscordColor Goldenrod { get; } = new(0xDAA520);
        
        /// <summary>
        /// Azure.
        /// </summary>
        public static DiscordColor Azure { get; } = new(0x007FFF);

        /// <summary>
        /// Rose.
        /// </summary>
        public static DiscordColor Rose { get; } = new(0xFF007F);

        /// <summary>
        /// Spring green.
        /// </summary>
        public static DiscordColor SpringGreen { get; } = new(0x00FF7F);

        /// <summary>
        /// Chartreuse. A very vibrant green color.
        /// </summary>
        public static DiscordColor Chartreuse { get; } = new(0x7FFF00);
        
        public static DiscordColor Orange { get; } = new(0xFFA500);

        public static DiscordColor Purple { get; } = new(0x800080);
        
        public static DiscordColor Violet { get; } = new(0xEE82EE);
        
        public static DiscordColor Brown { get; } = new(0xA52A2A);
        
        public static DiscordColor HotPink { get; } = new(0xFF69B4);
        
        public static DiscordColor Lilac { get; } = new(0xC8A2C8);

        /// <summary>
        /// Cornflower blue.
        /// </summary>
        public static DiscordColor CornflowerBlue { get; } = new(0x6495ED);

        /// <summary>
        /// Midnight blue.
        /// </summary>
        public static DiscordColor MidnightBlue { get; } = new(0x191970);

        /// <summary>
        /// Wheat.
        /// </summary>
        public static DiscordColor Wheat { get; } = new(0xF5DEB3);

        /// <summary>
        /// Indian red. It's a pinkish color.
        /// </summary>
        public static DiscordColor IndianRed { get; } = new(0xCD5C5C);

        /// <summary>
        /// Turquoise.
        /// </summary>
        public static DiscordColor Turquoise { get; } = new(0x30D5C8);

        /// <summary>
        /// Sap green.
        /// </summary>
        public static DiscordColor SapGreen { get; } = new(0x507D2A);

        // meme, specifically bob ross
        /// <summary>
        /// Phthalo blue.
        /// </summary>
        public static DiscordColor PhthaloBlue { get; } = new(0x000F89);
        
        /// <summary>
        /// Phthalo green.
        /// </summary>
        public static DiscordColor PhthaloGreen { get; } = new(0x123524);

        /// <summary>
        /// Sienna, a somewhat rusty looking color.
        /// </summary>
        public static DiscordColor Sienna { get; } = new(0x882D17);
        
        #endregion
	}
}