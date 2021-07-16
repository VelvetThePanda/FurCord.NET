using FurCord.NET.Net;
using Microsoft.Extensions.DependencyInjection;

namespace FurCord.NET
{
	public static class ClientContainerExtensions
	{
		public static IServiceCollection AddClient(this IServiceCollection collection, DiscordConfiguration config)
		{
			collection.AddTransient(_ => config);
			collection.AddSingleton<IWebSocketClient>(s => s.GetRequiredService<DiscordConfiguration>().WebSocketClientFactory());
			collection.AddSingleton<IRestClient, RestClient>(s => new RestClient(s.GetRequiredService<DiscordConfiguration>()));
			collection.AddSingleton<IDiscordClient, DiscordClient>();

			return collection;
		}
	}
}