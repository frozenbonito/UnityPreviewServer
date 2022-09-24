using UnityEditor;
using UnityEngine;

namespace UnityPreviewServer.Core
{
	[FilePath("PreviewServerPreferences.asset", FilePathAttribute.Location.PreferencesFolder)]
	public class Preferences : ScriptableSingleton<Preferences>
	{
		private bool _enabled;

		[SerializeField]
		private uint _defaultPort = Constants.DefaultPort;

		public uint DefaultPort
		{
			get
			{
				return _defaultPort;
			}
			private set
			{
				_defaultPort = value;
			}
		}

		void OnEnable()
		{
			hideFlags &= ~HideFlags.NotEditable;
		}

		void OnValidate()
		{
			if (DefaultPort < Constants.MinPort)
			{
				DefaultPort = Constants.MinPort;
			}
			if (DefaultPort > Constants.MaxPort)
			{
				DefaultPort = Constants.MaxPort;
			}

			if (_enabled)
			{
				Save(true);
			}
		}

	}
}
