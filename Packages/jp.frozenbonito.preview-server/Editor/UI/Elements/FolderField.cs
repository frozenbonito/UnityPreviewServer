using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace UnityPreviewServer.UI.Elements
{
	public class FolderField : VisualElement
	{
		private string _value;
		private readonly Label _label;
		private readonly TextElement _text;
		private readonly Button _button;

		public string BindingPath
		{
			get
			{
				return _text.bindingPath;
			}
			set
			{
				_text.bindingPath = value;
			}
		}

		public string Label
		{
			get
			{
				return _value;
			}
			set
			{
#if !UNITY_2023_1_OR_NEWER
				_label.text = value.Replace("\\", "\\\\");
#endif
				_value = value;
			}
		}

		public string PanelTitle { get; set; } = "Select folder";

		public string Value
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

		public FolderField()
		{
			var style = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{Constants.PackagePath}/Editor/UI/Elements/Styles/FolderField.uss");
			styleSheets.Add(style);

			var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{Constants.PackagePath}/Editor/UI/Elements/UXML/FolderField.uxml");
			var container = visualTree.Instantiate();
			Add(container);

			_label = container.Q<Label>("label");
			_text = container.Q<TextElement>("text");
			_button = container.Q<Button>("button");

			_button.clicked += OnButtonClick;
		}

		public FolderField(string label) : this()
		{
			Label = label;
		}

		protected virtual void OnButtonClick()
		{
			var folder = EditorUtility.OpenFolderPanel(PanelTitle, GetBaseFolder(), GetDefaultName());
			if (folder == "")
			{
				return;
			}
			Value = folder;
		}

		public new class UxmlFactory : UxmlFactory<FolderField, UxmlTraits> { }

		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			UxmlStringAttributeDescription _bindingPath = new UxmlStringAttributeDescription()
			{
				name = "binding-path"
			};
			UxmlBoolAttributeDescription _disabled = new UxmlBoolAttributeDescription()
			{
				name = "disabled"
			};
			UxmlStringAttributeDescription _label = new UxmlStringAttributeDescription()
			{
				name = "label"
			};
			UxmlStringAttributeDescription _panelTitle = new UxmlStringAttributeDescription()
			{
				name = "panel-title"
			};
			UxmlStringAttributeDescription _value = new UxmlStringAttributeDescription()
			{
				name = "value"
			};

			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get { yield break; }
			}

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);

				var folderField = ve as FolderField;
				folderField.BindingPath = _bindingPath.GetValueFromBag(bag, cc);
				folderField.Label = _label.GetValueFromBag(bag, cc);
				folderField.PanelTitle = _panelTitle.GetValueFromBag(bag, cc);
				folderField.Value = _value.GetValueFromBag(bag, cc);
			}
		}

		private string GetBaseFolder()
		{
			if (String.IsNullOrEmpty(Value))
			{
				return Utils.GetProjectPath();
			}
			return Path.GetDirectoryName(Value);
		}

		private string GetDefaultName()
		{
			if (String.IsNullOrEmpty(Value))
			{
				return string.Empty;
			}
			return Path.GetFileName(Value);
		}
	}
}
