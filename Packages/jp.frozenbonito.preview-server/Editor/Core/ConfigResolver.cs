using System.IO;

namespace UnityPreviewServer.Core
{
	public static class ConfigResolver
	{
		public static string GetBuildLocation()
		{
			if (UserSettings.instance.UseDefaultBuildLocation)
			{
				return Path.Join(Utils.GetProjectPath(), ProjectSettings.instance.DefaultBuildLocation);
			}
			return UserSettings.instance.BuildLocation;
		}

		public static uint GetPort()
		{
			if (UserSettings.instance.UseDefaultPort)
			{
				return Preferences.instance.DefaultPort;
			}
			return UserSettings.instance.Port;
		}
	}
}
