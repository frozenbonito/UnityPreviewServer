using UnityEditor;
using UnityEngine;

namespace UnityPreviewServer.Core
{
	[FilePath("UserSettings/PreviewServerSettings.asset", FilePathAttribute.Location.ProjectFolder)]
	public class UserSettings : ScriptableSingleton<UserSettings>
	{
		private bool _enabled;

		[SerializeField]
		private bool _useDefaultBuildLocation = true;

		[SerializeField]
		private string _buildLocation;

		[SerializeField]
		private bool _useDefaultPort = true;

		[SerializeField]
		private uint _port = Constants.DefaultPort;

		public bool UseDefaultBuildLocation
		{
			get
			{
				return _useDefaultBuildLocation;
			}
			private set
			{
				_useDefaultBuildLocation = value;
			}
		}

		public string BuildLocation
		{
			get
			{
				return _buildLocation;
			}
			private set
			{
				_buildLocation = value;
			}
		}

		public bool UseDefaultPort
		{
			get
			{
				return _useDefaultPort;
			}
			private set
			{
				_useDefaultPort = value;
			}
		}

		public uint Port
		{
			get
			{
				return _port;
			}
			private set
			{
				_port = value;
			}
		}

		public void Save()
		{
			Save(true);
		}

		void OnEnable()
		{
			BuildLocation = Utils.GetDefaultBuildLocation();
			_enabled = true;
		}

		void OnValidate()
		{
			if (Port < Constants.MinPort)
			{
				Port = Constants.MinPort;
			}
			if (Port > Constants.MaxPort)
			{
				Port = Constants.MaxPort;
			}

			if (_enabled)
			{
				Save(true);
			}
		}
	}
}
