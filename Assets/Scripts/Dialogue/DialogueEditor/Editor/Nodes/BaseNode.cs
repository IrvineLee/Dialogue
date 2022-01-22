using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

using DialogueEditor.Editor.GraphView;
using DialogueEditor.Runtime;
using DialogueEditor.Runtime.Classes;
using DialogueEditor.Runtime.Enums.Language;
using System.Linq;

namespace DialogueEditor.Editor.Nodes
{
	public class BaseNode : Node
	{
		protected string nodeGuid;
		protected DialogueGraphView graphView;
		protected DialogueEditorWindow editorWindow;
		protected Vector2 defaultNodeSide = new Vector2(200, 250);

		List<LanguageGenericHolder.OfText> languageGenericTextList = new List<LanguageGenericHolder.OfText>();
		List<LanguageGenericHolder.OfAudioClip> languageGenericAudioList = new List<LanguageGenericHolder.OfAudioClip>();

		string nodeStyleSheet = "USS/Nodes/NodeStyleSheet";

		public string NodeGuid { get => nodeGuid; }

		public BaseNode()
		{
			StyleSheet styleSheet = Resources.Load<StyleSheet>(nodeStyleSheet);
			styleSheets.Add(styleSheet);
		}

		public virtual void LoadValueIntoField() { }

		public void SetNodeGuid(string nodeGuid) { this.nodeGuid = nodeGuid; }

		protected void Initialize(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView, string title, StyleSheet styleSheetPath)
		{
			this.editorWindow = editorWindow;
			this.graphView = graphView;

			SetPosition(new Rect(position, defaultNodeSide));
			nodeGuid = Guid.NewGuid().ToString();

			this.title = title;
			styleSheets.Add(styleSheetPath);
		}

		#region Port
		/// <summary>
		/// Add a port to the outputContainer.
		/// </summary>
		public Port AddOutputPort(string name, Port.Capacity capacity = Port.Capacity.Single)
		{
			Port outputPort = GetPortInstance(Direction.Output, capacity);
			outputPort.portName = name;
			outputContainer.Add(outputPort);

			return outputPort;
		}

		/// <summary>
		/// Add a port to the inputContainer.
		/// </summary>
		public Port AddInputPort(string name, Port.Capacity capacity = Port.Capacity.Multi)
		{
			Port inputPort = GetPortInstance(Direction.Input, capacity);
			inputPort.portName = name;
			inputContainer.Add(inputPort);

			return inputPort;
		}

		/// <summary>
		/// Create a port instance.
		/// </summary>
		public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
		{
			return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
		}
		#endregion

		#region Reload Language
		/// <summary>
		/// Reload languages to the current selected language.
		/// </summary>
		public virtual void ReloadLanguage()
		{
			foreach (LanguageGenericHolder.OfText textHolder in languageGenericTextList)
			{
				Reload_TextLanguage(textHolder.InputText, textHolder.TextField, textHolder.PlaceholderText);
			}
			foreach (LanguageGenericHolder.OfAudioClip audioClipHolder in languageGenericAudioList)
			{
				Reload_AudioClipLanguage(audioClipHolder.InputAudioClip, audioClipHolder.ObjectField);
			}
		}

		/// <summary>
		/// Reload all the text in the TextField to the current selected language.
		/// </summary>
		/// <param name="inputText"> List of LanguageGeneric<string> </param>
		/// <param name="textField"> The TextField that is to be reloaded in. </param>
		/// <param name="placeholderText"> Text that will be displayed if the text field is empty. </param>
		protected void Reload_TextLanguage(List<LanguageGeneric<string>> inputTextList, TextField textField, string placeholderText = "")
		{
			// Reload Text.
			textField.RegisterValueChangedCallback(value =>
			{
				SetLanguageGenericType(inputTextList, value.newValue);
			});
			textField.SetValueWithoutNotify(GetLanguageGenericType(inputTextList));

			SetPlaceholderText(textField, placeholderText);
		}

		/// <summary>
		/// Reload all the audio clips in the ObjectField to the current selected language.
		/// </summary>
		/// <param name="inputText"> List of LanguageGeneric<AudioClip> </param>
		/// <param name="objectField"> The ObjectField that is to be reloaded in. </param>
		protected void Reload_AudioClipLanguage(List<LanguageGeneric<AudioClip>> inputAudioClipList, ObjectField objectField)
		{
			// Reload Audio.
			objectField.RegisterValueChangedCallback(value =>
			{
				SetLanguageGenericType(inputAudioClipList, (AudioClip)value.newValue);
			});
			objectField.SetValueWithoutNotify(GetLanguageGenericType(inputAudioClipList));
		}
		#endregion

		#region New Label
		/// <summary>
		/// Get new label. Optional USS naming.
		/// </summary>
		protected Label GetNewLabel(string labelName, Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			Label labelText = new Label(labelName);
			AddToClassList(labelText, USS01, USS02);
			boxContainer?.Add(labelText);

			return labelText;
		}
		#endregion

		#region New Button
		/// <summary>
		/// Get new button. Optional USS naming.
		/// </summary>
		protected Button GetNewButton(string buttonText, Action clickedAction = default, Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			Button button = new Button();
			button.text = buttonText;
			if (clickedAction != default) button.clicked += () => clickedAction();

			AddToClassList(button, USS01, USS02);
			boxContainer?.Add(button);
			return button;
		}
		#endregion

		#region New Image
		/// <summary>
		/// Get Image Field. Optional USS naming.
		/// </summary>
		protected Image GetNewImage(Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			Image imageField = new Image();
			AddToClassList(imageField, USS01, USS02);
			boxContainer?.Add(imageField);

			return imageField;
		}
		#endregion

		#region New Int Field
		/// <summary>
		/// Get Integer Field. Optional USS naming.
		/// </summary>
		protected IntegerField GetNewIntegerField(IntContainer inputValue, Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			IntegerField integerField = new IntegerField();
			integerField = (IntegerField)GetNewField(integerField, inputValue, USS01, USS02);
			boxContainer?.Add(integerField);

			return integerField;
		}
		#endregion

		#region New Float Field
		/// <summary>
		/// Get Float Field. Optional USS naming.
		/// </summary>
		protected FloatField GetNewFloatField(FloatContainer inputValue, Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			FloatField floatField = new FloatField();
			floatField = (FloatField)GetNewField(floatField, inputValue, USS01, USS02);
			boxContainer?.Add(floatField);

			return floatField;
		}
		#endregion

		#region New Text Field
		/// <summary>
		/// Get Text Field. Optional USS naming.
		/// </summary>
		protected TextField GetNewTextField(StringContainer inputValue, string placeholderText, Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			TextField textField = new TextField();
			textField = (TextField)GetNewField(textField, inputValue, USS01, USS02);
			boxContainer?.Add(textField);

			SetPlaceholderText(textField, placeholderText);
			return textField;
		}
		#endregion

		#region New Sprite Object Field
		/// <summary>
		/// Get Sprite Object Field. Optional USS naming.
		/// </summary>
		protected ObjectField GetNewObjectField_Sprite(SpriteContainer inputValue, Image imagePreview, Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			ObjectField objectField = new ObjectField()
			{
				objectType = typeof(Sprite),
				allowSceneObjects = false,
				value = inputValue.Value,
			};
			boxContainer?.Add(objectField);

			Action valueChangedCallback = () => { imagePreview.image = (inputValue.Value != null) ? inputValue.Value.texture : null; };
			objectField = GetNewField(objectField, inputValue, valueChangedCallback, USS01, USS02);
			imagePreview.image = (inputValue.Value != null) ? inputValue.Value.texture : null;

			return objectField;
		}
		#endregion

		#region New Popup Field
		/// <summary>
		/// Get a generic Popup Field from a list. Optional USS naming.
		/// </summary>
		protected PopupField<string> GetNewPopupField<T>(Container<T> inputValue, List<(string, Color)> tuppleList, Box boxContainer = null,
														 string USS01 = "", string USS02 = "")
		{
			PopupField<string> popupField = new PopupField<string>(tuppleList.Select(tuple => tuple.Item1).ToList(), 0);

			Action<Color> act = (color) =>
			{
				var style = popupField[0].style;

				style.borderLeftColor = color;
				style.borderRightColor = color;
				style.borderTopColor = color;
				style.borderBottomColor = color;
			};

			popupField.RegisterValueChangedCallback(value =>
			{
				inputValue.SetValue((T)(object)value.newValue);

				(string, Color) singleTupple = tuppleList.Single(name => Equals(name.Item1.ToString(), value.newValue.ToString()));
				act(singleTupple.Item2);
			});
			popupField.SetValueWithoutNotify(popupField.value);

			AddToClassList(popupField, USS01, USS02);
			boxContainer?.Add(popupField);

			return popupField;
		}
		#endregion

		#region---- Custom Events ----
		/// <summary>
		/// Get Text Field that uses a List<LanguageGeneric<string>>. Optional USS naming.
		/// </summary>
		protected TextField GetNewTextField_TextLanguage(List<LanguageGeneric<string>> textList, string placeholder,
														 Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			foreach (LanguageType language in Enum.GetValues(typeof(LanguageType)))
			{
				textList.Add(new LanguageGeneric<string>(language, ""));
			}

			TextField textField = new TextField();
			textField.multiline = true;
			textField.RegisterValueChangedCallback(value => SetLanguageGenericType(textList, value.newValue));
			textField.SetValueWithoutNotify(GetLanguageGenericType(textList));
			boxContainer?.Add(textField);

			languageGenericTextList.Add(new LanguageGenericHolder.OfText(textList, textField, placeholder));

			AddToClassList(textField, USS01, USS02);
			return textField;
		}

		/// <summary>
		/// Get Object Field that uses a List<LanguageGeneric<AudioClip>>. Optional USS naming.
		/// </summary>
		protected ObjectField GetNewTextField_AudioClipLanguage(List<LanguageGeneric<AudioClip>> audioClipList, Box boxContainer,
																string USS01 = "", string USS02 = "")
		{
			foreach (LanguageType language in Enum.GetValues(typeof(LanguageType)))
			{
				audioClipList.Add(new LanguageGeneric<AudioClip>(language, null));
			}

			ObjectField objectField = new ObjectField()
			{
				objectType = typeof(AudioClip),
				allowSceneObjects = false,
				value = GetLanguageGenericType(audioClipList),
			};

			objectField.RegisterValueChangedCallback(value => SetLanguageGenericType(audioClipList, (AudioClip)value.newValue));
			objectField.SetValueWithoutNotify(GetLanguageGenericType(audioClipList));
			boxContainer?.Add(objectField);

			languageGenericAudioList.Add(new LanguageGenericHolder.OfAudioClip(audioClipList, objectField));

			AddToClassList(objectField, USS01, USS02);
			return objectField;
		}
		#endregion

		#region ---- Protected Functions ----
		/// <summary>
		/// Set the language generic type after changing language.
		/// </summary>
		protected void SetLanguageGenericType<T>(List<LanguageGeneric<T>> languageGeneric, T newValue)
		{
			languageGeneric.Find(value => value.LanguageType == editorWindow.SelectedLanguage).SetLanguageGenericType(newValue);
		}

		/// <summary>
		/// Get the current language generic type.
		/// </summary>
		protected T GetLanguageGenericType<T>(List<LanguageGeneric<T>> languageGeneric)
		{
			return (T)(object)languageGeneric.Find(value => value.LanguageType == editorWindow.SelectedLanguage).LanguageGenericType;
		}

		/// <summary>
		/// Generic GetNewField that returns a Base Field.
		/// </summary>
		protected BaseField<T> GetNewField<T>(BaseField<T> basefield, Container<T> inputValue, string USS01 = "", string USS02 = "")
		{
			basefield.RegisterValueChangedCallback(value => inputValue.SetValue(value.newValue));
			basefield.SetValueWithoutNotify(inputValue.Value);

			AddToClassList(basefield, USS01, USS02);
			return basefield;
		}

		/// <summary>
		/// Generic GetNewField that returns an Object Field.
		/// </summary>
		protected ObjectField GetNewField<T>(ObjectField objectField, Container<T> inputValue, Action valueChangedCallback = default, string USS01 = "", string USS02 = "")
		{
			objectField.RegisterValueChangedCallback(value =>
			{
				inputValue.SetValue((T)(object)value.newValue);
				valueChangedCallback?.Invoke();
			});
			AddToClassList(objectField, USS01, USS02);

			return objectField;
		}

		/// <summary>
		/// Generic Enum Field
		/// </summary>
		protected EnumField GetNewEnumField_Generic<T>(EnumContainer<T> inputValue, Action valueChangedCallback = default,
											 Box boxContainer = null, string USS01 = "", string USS02 = "") where T : Enum
		{
			EnumField enumField = new EnumField()
			{
				value = inputValue.Value,
			};
			enumField.Init(inputValue.Value);

			enumField.RegisterValueChangedCallback(value =>
			{
				inputValue.SetValue((T)value.newValue);
				valueChangedCallback?.Invoke();
			});
			enumField.SetValueWithoutNotify(inputValue.Value);

			AddToClassList(enumField, USS01, USS02);
			boxContainer?.Add(enumField);

			inputValue.SetEnumField(enumField);
			return enumField;
		}

		/// <summary>
		/// Add to ClassList
		/// </summary>
		protected void AddToClassList(VisualElement visualElement, string USS01 = "", string USS02 = "")
		{
			visualElement.AddToClassList(USS01);
			visualElement.AddToClassList(USS02);
		}

		/// <summary>
		/// Set a placeholder text in the TextField.
		/// </summary>
		protected void SetPlaceholderText(TextField textField, string placeholder)
		{
			string placeholderClass = TextField.ussClassName + "__placeholder";

			if (!string.IsNullOrEmpty(textField.text))
				textField.RemoveFromClassList(placeholderClass);

			OnFocusOut();

			textField.RegisterCallback<FocusInEvent>(evt => OnFocusIn());
			textField.RegisterCallback<FocusOutEvent>(evt => OnFocusOut());

			void OnFocusIn()
			{
				if (textField.ClassListContains(placeholderClass))
				{
					textField.value = string.Empty;
					textField.RemoveFromClassList(placeholderClass);
				}
			}

			void OnFocusOut()
			{
				if (string.IsNullOrEmpty(textField.text))
				{
					textField.SetValueWithoutNotify(placeholder);
					textField.AddToClassList(placeholderClass);
				}
			}
		}
		#endregion
	}
}