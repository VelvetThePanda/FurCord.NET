using System.Threading.Tasks;
using FurCord.NET.Entities;

namespace FurCord.NET.Net
{
	public class DiscordClient : IDiscordClient
	{
		
		public int Ping { get; }
		public ClientState State { get; }
		public IUser CurrentUser { get; }
		public async Task ConnectAsync() { }
		public async Task DisconnectAsync() { }
		public async Task<IMessage> SendMessageAsync(IUser user, IMessage message)
		{
			return null;
		}

		public DiscordClient(DiscordConfiguration config)
		{
				
		}
	}
}