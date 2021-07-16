﻿using System.IO;
using System.Threading.Tasks;
using FurCord.NET.Net;
using FurCord.NET.Net.Enums;

namespace FurCord.NET.Testing
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var client = new DiscordClient(new(File.ReadAllText("./token.txt"))
			{
				Intents = GatewayIntents.AllUnprivileged
			});

			await client.ConnectAsync();
			
			await Task.Delay(-1);
		}
	}
}