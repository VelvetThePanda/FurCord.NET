using System.Collections.Generic;
using System.Threading.Tasks;
using FurCord.NET.Entities;

namespace FurCord.NET.Net
{
	public interface IDiscordClient
	{
		public int Ping { get; }

		public ClientState State { get; }
		
		internal DiscordConfiguration Configuration { get; }
		
		public IUser CurrentUser { get; }
		
		public IReadOnlyDictionary<ulong, IGuild> Guilds { get; }
		
		public Task ConnectAsync();
		public Task DisconnectAsync();

		public Task<IMessage> SendMessageAsync(IUser user, IMessage message);
		
	}
}