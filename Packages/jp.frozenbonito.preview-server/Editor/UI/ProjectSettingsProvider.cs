using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityPreviewServer.Core;

namespace UnityPreviewServer.UI
{
	public class ProjectSettingsProvider : SettingsProvider
	{
		private const string SettingsPath = "Project/Preview Server";
		public ProjectSettingsProvider() : base(SettingsPath, SettingsScope.Project, null) { }

		[SettingsProvider]
		public static SettingsProvider CreateProjectSettingsProvider()
		{
			return new ProjectSettingsProvider();
		}

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			Preferences.instance.hideFlags &= ~HideFlags.NotEditable;

			var globalStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{Constants.PackagePath}/Editor/UI/Styles/Global.uss");
			rootElement.styleSheets.Add(globalStyle);

			var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{Constants.PackagePath}/Editor/UI/UXML/ProjectSettings.uxml");
			rootElement.Add(visualTree.Instantiate());

			var serializedObject = new SerializedObject(ProjectSettings.instance);
			rootElement.Bind(serializedObject);
		}
	}
}
