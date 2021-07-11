namespace FurCord.NET.Entities
{
	public interface IChannel : ISnowflake
	{
		public string Name { get; }
		public IGuild Guild { get; }
	}
}