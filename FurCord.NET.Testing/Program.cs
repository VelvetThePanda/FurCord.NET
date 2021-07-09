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
			var rest = new RestClient(File.ReadAllText("token.txt"));
			for (int i = 0; i < 50; i++)
			{
				var req = new JsonRestRequest<object>("channels/:channel_id/messages", new {content = $"This is an automated test of FurCord.NET's REST ratelimit handler. [{i}/50]"}, RestMethod.POST, new() {["channel_id"] = 794055225517408277});
				await rest.DoRequestAsync(req);
				await req.Response;
			}
			await Task.Delay(-1);
		}
	}
}