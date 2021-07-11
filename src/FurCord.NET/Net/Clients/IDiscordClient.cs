using System.Threading.Tasks;
using FurCord.NET.Entities;

namespace FurCord.NET
{
	public interface IDiscordClient
	{
		public int Ping { get; }
		
		public ClientState State { get; }
		
		public IUser CurrentUser { get; }
		
		public Task ConnectAsync();
		public Task DisconnectAsync();

	}
}