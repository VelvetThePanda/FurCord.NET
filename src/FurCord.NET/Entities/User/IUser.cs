using FurCord.NET.Entities.Converters;
using Newtonsoft.Json;

namespace FurCord.NET.Entities
{
	public interface IUser : ISnowflake
	{
		public string Username { get; }
		
		string AvatarHash { get; }
		
		public string Discriminator { get; }
		
		public int Flags { get; }
		
		public bool Bot { get; }
	}
}