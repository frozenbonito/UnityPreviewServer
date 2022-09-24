using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using UnityPreviewServer.Core;

namespace UnityPreviewServer.UI.Elements
{
	public class Log : VisualElement
	{
		public static string InfoClass = "info-log";
		public static string ErrorClass = "error-log";

		public readonly Guid Id;

		private readonly TextElement _text;

		public LogLevel Level
		{
			get
			{
				if (GetClasses().Contains(ErrorClass))
				{
					return LogLevel.Error;
				}
				return LogLevel.Info;
			}
			set
			{
				switch (value)
				{
					case LogLevel.Error:
						RemoveFromClassList(InfoClass);
						AddToClassList(ErrorClass);
						break;
					default:
						RemoveFromClassList(ErrorClass);
						AddToClassList(InfoClass);
						break;
				}
			}
		}

		public string Message
		{
			get
			{
				return _text.text;
			}
			set
			{
				_text.text = value;
			}
		}

		public Log()
		{
			Id = Guid.NewGuid();
			Level = LogLevel.Info;

			var style = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{Constants.PackagePath}/Editor/UI/Elements/Styles/Log.uss");
			styleSheets.Add(style);

			var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{Constants.PackagePath}/Editor/UI/Elements/UXML/Log.uxml");
			var container = visualTree.Instantiate();
			Add(container);

			_text = container.Q<TextElement>("text");
#if UNITY_2022_2_OR_NEWER
			_text.selection.isSelectable = true;
#elif UNITY_2022_1_OR_NEWER
			_text.isSelectable = true;
#endif
		}

		public Log(LogLevel level, string message) : this()
		{
			Level = level;
			Message = message;
		}

		public new class UxmlFactory : UxmlFactory<Log, UxmlTraits> { }

		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			UxmlEnumAttributeDescription<LogLevel> _level = new UxmlEnumAttributeDescription<LogLevel>()
			{
				name = "level"
			};
			UxmlStringAttributeDescription _message = new UxmlStringAttributeDescription()
			{
				name = "message"
			};

			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get { yield break; }
			}

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);

				var logEntry = ve as Log;
				logEntry.Level = _level.GetValueFromBag(bag, cc);
				logEntry.Message = _message.GetValueFromBag(bag, cc);
			}
		}
	}
}
