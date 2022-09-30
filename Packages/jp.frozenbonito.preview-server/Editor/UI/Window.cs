using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityPreviewServer.Core;
using UnityPreviewServer.UI.Elements;

namespace UnityPreviewServer.UI
{
	public class Window : EditorWindow
	{
		[NonSerialized]
		private int _logCursor = 0;
		[NonSerialized]
		private Guid _lastLogId = Guid.Empty;
		[NonSerialized]
		private bool _initialized = false;

		private FolderField _buildLocationField;
		private PropertyField _portField;
		private Button _startStopButton;
		private Button _clearButton;
		private LogViewer _logViewer;

		public void CreateGUI()
		{
			UserSettings.instance.hideFlags &= ~HideFlags.NotEditable;

			var root = rootVisualElement;

			var globalStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{Constants.PackagePath}/Editor/UI/Styles/Global.uss");
			root.styleSheets.Add(globalStyle);

			var style = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{Constants.PackagePath}/Editor/UI/Styles/Window.uss");
			root.styleSheets.Add(style);

			var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{Constants.PackagePath}/Editor/UI/UXML/Window.uxml");
			var container = visualTree.Instantiate();
			root.Add(container);

			var serializedObject = new SerializedObject(UserSettings.instance);
			root.Bind(serializedObject);

			_buildLocationField = container.Q<FolderField>("buildLocationField");
			_portField = container.Q<PropertyField>("portField");
			_startStopButton = container.Q<Button>("startStopButton");
			_clearButton = container.Q<Button>("clearButton");
			_logViewer = container.Q<LogViewer>("logViewer");

			_startStopButton.clicked += OnStartStopButtonClick;
			_clearButton.clicked += OnClearButtonClick;

			_initialized = true;
		}

		void Update()
		{
			if (!_initialized)
			{
				return;
			}

			_buildLocationField.SetEnabled(!UserSettings.instance.UseDefaultBuildLocation);
			_portField.SetEnabled(!UserSettings.instance.UseDefaultPort);

			_startStopButton.text = Server.Instance.IsRunning ? "Stop" : "Start";

			if (_logCursor == 0 && !ServerLogs.instance.HasLogs && _logViewer.HasLogs)
			{
				Reset();
				return;
			}

			if (_lastLogId != _logViewer.LastLogId)
			{
				_lastLogId = _logViewer.ScrollToLastLog();
			}

			var logEntries = ServerLogs.instance.GetLatestEntries(_logCursor);
			if (logEntries.Count == 0)
			{
				return;
			}

			_logCursor += logEntries.Count;
			logEntries.ForEach(x =>
			{
				_logViewer.AddLog(x);
			});
		}

		void Reset()
		{
			_logViewer?.ClearLogs();
			_logCursor = 0;
			_lastLogId = Guid.Empty;
		}

		private void OnStartStopButtonClick()
		{
			if (Server.Instance.IsRunning)
			{
				Server.Instance.Stop();
				return;
			}

			Reset();
			var buildLocation = ConfigResolver.GetBuildLocation();
			var port = ConfigResolver.GetPort();
			Server.Instance.Start(buildLocation, port, true);
		}

		private void OnClearButtonClick()
		{
			ServerLogs.instance.Clear();
			Reset();
		}
	}
}
