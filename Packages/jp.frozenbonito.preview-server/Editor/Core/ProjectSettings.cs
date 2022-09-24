using UnityEditor;
using UnityEngine;

namespace UnityPreviewServer.Core
{
	[FilePath("ProjectSettings/PreviewServerSettings.asset", FilePathAttribute.Location.ProjectFolder)]
	public class ProjectSettings : ScriptableSingleton<ProjectSettings>
	{
		private bool _enabled;

		[SerializeField]
		private string _defaultBuildLocation = Constants.DefaultBuildLocation;

		public string DefaultBuildLocation
		{
			get
			{
				return _defaultBuildLocation;
			}
			private set
			{
				_defaultBuildLocation = value;
			}
		}

		void OnEnable()
		{
			hideFlags &= ~HideFlags.NotEditable;
			_enabled = true;
		}

		void OnValidate()
		{
			if (_enabled)
			{
				Save(true);
			}
		}
	}
}
