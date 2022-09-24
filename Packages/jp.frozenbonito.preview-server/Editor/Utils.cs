using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityPreviewServer
{
	public static class Utils
	{
		public static string GetProjectPath()
		{
			return Path.GetDirectoryName(Application.dataPath);
		}

		public static bool IsInProject(string path)
		{
			return Path.GetFullPath(path).StartsWith(GetProjectPath());
		}

		public static string ToAssetPath(string path)
		{
			var rel = Path.GetRelativePath(GetProjectPath(), path);
			return rel.Replace(Path.DirectorySeparatorChar, '/');
		}

		public static string GetDefaultBuildLocation()
		{
			var location = EditorUserBuildSettings.GetBuildLocation(BuildTarget.WebGL);
			if (!String.IsNullOrEmpty(location))
			{
				return location;
			}
			return Path.Join(GetProjectPath(), Constants.DefaultBuildLocation);
		}
	}
}
