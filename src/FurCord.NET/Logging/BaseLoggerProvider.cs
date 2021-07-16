using Microsoft.Extensions.Logging;

namespace FurCord.NET
{
	internal sealed class BaseLoggerProvider : ILoggerProvider
	{
		private readonly LogLevel _minimumLevel;
		private readonly string _timestampFormat;
		
		public void Dispose() { }
		public ILogger CreateLogger(string categoryName) => new BaseLogger(categoryName, _minimumLevel, _timestampFormat);
		internal BaseLoggerProvider(LogLevel minLevel = LogLevel.Information, string timestampFormat = "yyyy-MM-dd HH:mm:ss")
		{
			_minimumLevel = minLevel;
			_timestampFormat = timestampFormat;
		}
	}
}