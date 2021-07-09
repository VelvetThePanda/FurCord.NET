using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FurCord.NET.Enums;

namespace FurCord.NET.Testing
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var rest = new RestClient(new("https://discord.com/api/v9"), "FurCord.NET / 0.1a (VelvetThePanda)", File.ReadAllText("token.txt"));
			var req = new JsonRestRequest<object>("channels/:channel_id/messages", new { content = "Ratelimit test!"}, RestMethod.POST, new() {["channel_id"] = 794055225517408277});

			var sw = Stopwatch.StartNew();
			for (int i = 0; i < 12; i++)
			{
				await rest.DoRequestAsync(req);
				var res = await req.Response;
				
				sw.Stop();
				Console.WriteLine($"Request returned {res.ResponseCode} in {sw.ElapsedMilliseconds}");
				
				if (res.ResponseCode is 400)
					Console.WriteLine(res.Content);
				
				sw.Restart();
			}
			
			
			
			await Task.Delay(-1);
		}
	}
}