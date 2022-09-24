using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using UnityPreviewServer.Core;

namespace UnityPreviewServer.UI.Elements
{
	public class LogViewer : VisualElement
	{
		private readonly ScrollView _logView;

		public Guid LastLogId
		{
			get
			{
				if (!HasLogs)
				{
					return Guid.Empty;
				}

				var last = _logView.Children().Last() as Log;
				return last.Id;
			}
		}

		public bool HasLogs
		{
			get
			{
				return (_logView?.childCount ?? 0) > 0;
			}
		}

		public LogViewer()
		{
			var style = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{Constants.PackagePath}/Editor/UI/Elements/Styles/LogViewer.uss");
			styleSheets.Add(style);

			var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{Constants.PackagePath}/Editor/UI/Elements/UXML/LogViewer.uxml");
			var container = visualTree.Instantiate();
			Add(container);

			_logView = container.Q<ScrollView>("log-view");
		}

		public void AddLog(LogEntry entry)
		{
			var log = new Log(entry.Level, entry.Message);
			_logView.Add(log);
		}

		public void ClearLogs()
		{
			_logView.Clear();
		}

		public Guid ScrollToLastLog()
		{
			if (!HasLogs)
			{
				return Guid.Empty;
			}

			var last = _logView.Children().Last() as Log;
			_logView.ScrollTo(last);
			return last.Id;
		}

		public new class UxmlFactory : UxmlFactory<LogViewer, UxmlTraits> { }

		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get { yield break; }
			}

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
			}
		}
	}
}
