using System;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace UnityPreviewServer.UI.Elements
{
	public class FolderInProjectField : FolderField
	{
		public FolderInProjectField() : base() { }

		public FolderInProjectField(string label) : base(label) { }

		protected override void OnButtonClick()
		{
			var folder = EditorUtility.OpenFolderPanel(PanelTitle, GetBaseFolder(), GetDefaultName());
			if (folder == "" || !Utils.IsInProject(folder))
			{
				return;
			}
			Value = Utils.ToAssetPath(folder);
		}

		public new class UxmlFactory : UxmlFactory<FolderInProjectField, UxmlTraits> { }

		private string GetFullPath()
		{
			return Path.GetFullPath(Value);
		}

		private string GetBaseFolder()
		{
			if (String.IsNullOrEmpty(Value))
			{
				return Utils.GetProjectPath();
			}
			return Path.GetDirectoryName(GetFullPath());
		}

		private string GetDefaultName()
		{
			if (String.IsNullOrEmpty(Value))
			{
				return string.Empty;
			}
			return Path.GetFileName(GetFullPath());
		}
	}
}
