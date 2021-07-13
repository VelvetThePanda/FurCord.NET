namespace FurCord.NET.Entities
{
	public class Role : IRole
	{

		public ulong Id { get; internal set; }

		public string Name { get; internal set; }

		public Permissions Permissions { get; internal set; }
	}
}