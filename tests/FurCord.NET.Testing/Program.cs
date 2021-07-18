using System.IO;
using System.Threading.Tasks;
using FurCord.NET.Net;
using FurCord.NET.Net.Enums;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace FurCord.NET.Testing
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var container = new ServiceCollection();

			var logConfig = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.Console(outputTemplate: "[{Timestamp:h:mm:ss ff tt}] [{Level:u3}] [{SourceContext}] {Message:lj} {Exception:j}{NewLine}");
			
			container.AddLogging(l => l.AddSerilog(logConfig.CreateLogger()));
			
			container.AddFurCordClient(new(File.ReadAllText("./token.txt")) {Intents = GatewayIntents.All});
			var builtContainer = container.BuildServiceProvider();
			var client = builtContainer.GetRequiredService<IDiscordClient>();
			
			await client.ConnectAsync();

			var ws = builtContainer.GetRequiredService<IWebSocketClient>();

			await Task.Delay(7000);

			await client.Guilds[721518523704410202].Members[209279906280898562].SendMessageAsync("Poggers! FurCord.NET works!");
			
			
			await Task.Delay(-1);
		}
	}
}