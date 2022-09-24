using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;

namespace UnityPreviewServer.Core
{
	public class Server
	{
		private const string UrlLogPrefix = "server running at: ";

		private static Server _instance = new Server();

		private string _unisrvPath;
		private Process _process;

		public static Server Instance
		{
			get
			{
				return _instance;
			}
		}

		public bool IsRunning
		{
			get
			{
				return _process != null && !_process.HasExited;
			}
		}

		public string DocumentRoot { get; private set; }
		public uint Port { get; private set; }

		private Server()
		{
			_unisrvPath = GetUnisrvPath();
		}

		public void Start(string documentRoot, uint port, bool openBrowser)
		{
			if (IsRunning)
			{
				throw new Exception("A preview server is already running");
			}

			DocumentRoot = documentRoot;
			Port = port;

			_process?.Dispose();

			ServerLogs.instance.Clear();

			_process = new Process();
			_process.StartInfo = new ProcessStartInfo()
			{
				FileName = _unisrvPath,
				Arguments = $"-port {Port}",
				WorkingDirectory = DocumentRoot,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true,
			};

			_process.OutputDataReceived += (sender, e) =>
			{
				var line = e.Data.Trim();
				ServerLogs.instance.Add(new LogEntry(LogLevel.Info, line));

				if (openBrowser && line.StartsWith(UrlLogPrefix))
				{
					var url = line.Remove(0, UrlLogPrefix.Length);
					Process.Start(new ProcessStartInfo()
					{
						FileName = url,
						UseShellExecute = true,
					});
				}
			};
			_process.ErrorDataReceived += (sender, e) =>
			{
				var line = e.Data.Trim();
				ServerLogs.instance.Add(new LogEntry(LogLevel.Error, line));
			};

			UnityEngine.Debug.Log("Starting preview server...");
			ServerLogs.instance.Add(new LogEntry(LogLevel.Info, "Starting preview server..."));

			try
			{
				_process.Start();
			}
			catch (Exception e)
			{
				_process = null;
				throw e;
			}

			_process.BeginOutputReadLine();
			_process.BeginErrorReadLine();
		}

		public void Stop()
		{
			if (_process == null)
			{
				return;
			}

			if (!_process.HasExited)
			{
				UnityEngine.Debug.Log("Preview server stopped.");
				ServerLogs.instance.Add(new LogEntry(LogLevel.Info, "Preview server stopped."));
				_process.Kill();
			}

			_process.Dispose();

			_process = null;
			DocumentRoot = null;
			Port = 0;
		}

		[InitializeOnLoadMethod]
		private static void OnProjectLoadedInEditor()
		{
			EditorApplication.quitting += Server.Instance.Stop;
			AssemblyReloadEvents.beforeAssemblyReload += () =>
			{
				Server.Instance.Stop();
				ServerLogs.instance.Clear();
			};
		}

		private static string GetUnisrvPath()
		{
			var os = GetOS();
			if (String.IsNullOrEmpty(os))
			{
				throw new Exception("Unsupported os");
			}

			var arch = GetArch();
			if (String.IsNullOrEmpty(arch))
			{
				throw new Exception("Unsupported arch");
			}

			var bin = "unisrv";
			if (os == "windows")
			{
				bin += ".exe";
			}

			return Path.GetFullPath($"{Constants.PackagePath}/unisrv/{os}/{arch}/{bin}");
		}

		private static string GetOS()
		{
			var os = String.Empty;
#if UNITY_EDITOR_WIN
			os = "windows";
#elif UNITY_EDITOR_OSX
			os = "darwin";
#elif UNITY_EDITOR_LINUX
			os = "linux";
#endif
			return os;
		}

		private static string GetArch()
		{
			var arch = RuntimeInformation.OSArchitecture;
			switch (arch)
			{
				case Architecture.X64:
					return "amd64";
				case Architecture.Arm64:
					return "arm64";
				default:
					return string.Empty;
			}
		}
	}
}
