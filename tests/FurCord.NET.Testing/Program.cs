using System.IO;
using System.Threading.Tasks;
using FurCord.NET.Net;
using FurCord.NET.Net.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace FurCord.NET.Testing
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var container = new ServiceCollection();
			container.AddFurCordClient(new(File.ReadAllText("./token.txt")) {Intents = GatewayIntents.AllUnprivileged});
			var client = container.BuildServiceProvider().GetRequiredService<IDiscordClient>();
			
			await client.ConnectAsync();

			await Task.Delay(8000);
			
			await Task.Delay(-1);
		}
	}
}