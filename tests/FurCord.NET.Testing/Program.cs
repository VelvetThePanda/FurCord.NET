using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FurCord.NET.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FurCord.NET.Testing
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			//var rest = RestClient.CreateDefault(File.ReadAllText("./token.txt"));
			//var req = new RestRequest("guilds/:guild_id", RestMethod.GET, new() {["guild_id"] = 721518523704410202});
			//var res = await rest.DoRequestAsync<Guild>(req);

			var sconv = new SnowflakeDictionaryConverter<Member>();
			var guild = JObject.Parse(File.ReadAllText("./guild.json")).ToObject<Guild>();
			
			await Task.Delay(-1);
		}
	}
}