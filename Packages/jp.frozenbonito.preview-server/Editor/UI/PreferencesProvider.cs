using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityPreviewServer.Core;

namespace UnityPreviewServer.UI
{
	public class PreferencesProvider : SettingsProvider
	{
		private const string SettingsPath = "Preferences/Preview Server";

		public PreferencesProvider() : base(SettingsPath, SettingsScope.User, null) { }

		[SettingsProvider]
		public static SettingsProvider CreatePreferencesProvider()
		{
			return new PreferencesProvider();
		}

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			Preferences.instance.hideFlags &= ~HideFlags.NotEditable;

			var globalStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{Constants.PackagePath}/Editor/UI/Styles/Global.uss");
			rootElement.styleSheets.Add(globalStyle);

			var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{Constants.PackagePath}/Editor/UI/UXML/Preferences.uxml");
			rootElement.Add(visualTree.Instantiate());

			var serializedObject = new SerializedObject(Preferences.instance);
			rootElement.Bind(serializedObject);
		}
	}
}
