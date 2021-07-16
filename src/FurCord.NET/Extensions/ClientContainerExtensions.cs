using FurCord.NET.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace FurCord.NET
{
	/// <summary>
	/// A class containing various extension methods related to FurCord's various clients.
	/// </summary>
	public static class ClientContainerExtensions
	{
		/// <summary>
		/// Adds a default DiscordClient to the container. Registered type is of type <see cref="IDiscordClient"/>.
		/// </summary>
		/// <param name="collection">The service collection to add the client to.</param>
		/// <param name="config">The configuration to configure the client with.</param>
		/// <returns>The current service collection to chain calls with.</returns>
		public static IServiceCollection AddFurCordClient(this IServiceCollection collection, DiscordConfiguration config)
		{
			collection.AddLogging(); //Just in case.//
			
			collection.TryAddTransient(_ => config);
			collection.TryAddSingleton<IRestClient, RestClient>();
			collection.TryAddSingleton<IDiscordClient, DiscordClient>();
			collection.TryAddSingleton<IWebSocketClient>(s => s.GetRequiredService<DiscordConfiguration>().WebSocketClientFactory());
			
			return collection;
		}
	}
}