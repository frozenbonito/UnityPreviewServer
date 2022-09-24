using System.Collections.Generic;
using UnityEditor;

namespace UnityPreviewServer.Core
{
	public class ServerLogs : ScriptableSingleton<ServerLogs>
	{
		private List<LogEntry> _entries = new List<LogEntry>();

		public bool HasLogs
		{
			get
			{
				return _entries.Count > 0;
			}
		}

		public void Add(LogEntry entry)
		{
			_entries.Add(entry);
		}

		public void Clear()
		{
			_entries = new List<LogEntry>();
		}

		public LogEntry Get(int index)
		{
			return _entries[index];
		}

		public List<LogEntry> GetLatestEntries(int index)
		{
			return _entries.GetRange(index, _entries.Count - index);
		}
	}
}
