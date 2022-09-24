using System;

namespace UnityPreviewServer.Core
{
	[Serializable]
	public class LogEntry
	{
		public readonly LogLevel Level;
		public readonly string Message;

		public LogEntry(LogLevel level, string message)
		{
			Level = level;
			Message = message;
		}
	}

	public enum LogLevel
	{
		Info,
		Error
	}
}
