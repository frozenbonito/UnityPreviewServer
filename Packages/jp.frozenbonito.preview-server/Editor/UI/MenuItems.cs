using UnityEditor;
using UnityPreviewServer.Core;

namespace UnityPreviewServer.UI
{
	public static class MenuItems
	{
		[MenuItem("Preview Server/Start")]
		static void Start()
		{
			var buildLocation = ConfigResolver.GetBuildLocation();
			var port = ConfigResolver.GetPort();
			Server.Instance.Start(buildLocation, port, true);
		}

		[MenuItem("Preview Server/Start", true)]
		static bool ValidateStart()
		{
			return !Server.Instance.IsRunning;
		}

		[MenuItem("Preview Server/Stop")]
		static void Stop()
		{
			Server.Instance.Stop();
		}

		[MenuItem("Preview Server/Stop", true)]
		static bool ValidateStop()
		{
			return Server.Instance.IsRunning;
		}

		[MenuItem("Preview Server/Open Window...")]
		static void OpenWindow()
		{
			Window.GetWindow<Window>("Preview Server");
		}
	}
}
