using FurCord.NET.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FurCord.NET
{
	public static class ClientContainerExtensions
	{
		public static IServiceCollection AddClient(this IServiceCollection collection, DiscordConfiguration config)
		{
			collection
				.AddLogging() //Just in case.//
				.AddTransient(_ => config)
				.AddSingleton<IRestClient, RestClient>()
				.AddSingleton<IDiscordClient, DiscordClient>()
				.AddSingleton<IWebSocketClient>(s => s.GetRequiredService<DiscordConfiguration>().WebSocketClientFactory());
			
			return collection;
		}
	}
}