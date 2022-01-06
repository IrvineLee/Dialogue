using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

using DialogueEditor.Editor.GraphView;
using DialogueEditor.Runtime.Enums.Nodes;
using DialogueEditor.Runtime.Enums.Language;
using DialogueEditor.Runtime.Classes;
using DialogueEditor.Runtime.Classes.Data;

using Character.ScriptableObjects;

namespace DialogueEditor.Editor.Nodes
{
	public class DialogueNode : BaseNode
	{
		[SerializeField] DialogueInfo dialogueInfo = new DialogueInfo();

		public DialogueInfo DialogueInfo { get => dialogueInfo; }

		Image characterPotraitPreview;                          // Character potrait preview.

		PopupField<string> characterNamePopupField;             // Character name popup.
		PopupField<Sprite> characterPotraitPopupField;          // Character potrait popup.
		EnumField potraitFacingtDirectionField;					// Potrait facing direction.

		ObjectField audioClipField;                             // Audio clip field.
		TextField textLanguageField;                            // Dialogue text field.

		// Character profiles.
		CharacterProfilesSO characterProfiles;
		string characterProfilePath = "Character/CharacterProfiles";

		// List of popup choices.
		List<string> characterNameList = new List<string>();
		List<Sprite> characterSpriteList = new List<Sprite>();

		public DialogueNode() { }

		public DialogueNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			this.editorWindow = editorWindow;
			this.graphView = graphView;

			title = "Dialogue";
			SetPosition(new Rect(position, defaultNodeSide));
			nodeGuid = Guid.NewGuid().ToString();

			AddInputPort("Input");

			foreach (LanguageType language in Enum.GetValues(typeof(LanguageType)))
			{
				dialogueInfo.TextLanguageList.Add(new LanguageGeneric<string>(language, ""));
				dialogueInfo.AudioClipList.Add(new LanguageGeneric<AudioClip>(language, null));
			}

			CreateMainContainer();
		}

		public void LoadDialogueNode(DialogueNodeData node)
		{
			dialogueInfo.SetCharacterName(node.DialogueInfo.CharacterName);
			dialogueInfo.SetCharacterPotrait(node.DialogueInfo.CharacterPotrait);
			dialogueInfo.SetPotraitFacingDirection(node.DialogueInfo.PotraitFacingDirection);

			foreach (LanguageGeneric<string> languageGeneric in node.DialogueInfo.TextLanguageList)
			{
				dialogueInfo.TextLanguageList.Find(language => language.LanguageType == languageGeneric.LanguageType).SetLanguageGenericType(languageGeneric.LanguageGenericType);
			}

			foreach (LanguageGeneric<AudioClip> languageGeneric in node.DialogueInfo.AudioClipList)
			{
				dialogueInfo.AudioClipList.Find(language => language.LanguageType == languageGeneric.LanguageType).SetLanguageGenericType(languageGeneric.LanguageGenericType);
			}

			foreach (DialogueNodePort nodePort in node.DialogueInfo.DialogueNodePortList)
			{
				AddChoicePort(this, nodePort);
			}

			LoadValueIntoField();

			// Update the potrait preview.
			if (node.DialogueInfo.CharacterPotrait)
				characterPotraitPreview.image = node.DialogueInfo.CharacterPotrait.texture;

			// Update the popup choices.
			List<Sprite> spriteList = characterProfiles.GetCharacterSpriteList(dialogueInfo.CharacterName);
			if (spriteList != null)
				characterPotraitPopupField.choices = spriteList;
		}

		public override void ReloadLanguage()
		{
			// Text.
			textLanguageField.RegisterValueChangedCallback(value =>
			{
				SetLanguageGenericType(dialogueInfo.TextLanguageList, value.newValue);
			});
			textLanguageField.SetValueWithoutNotify(GetLanguageGenericType(dialogueInfo.TextLanguageList));

			// Audio.
			audioClipField.RegisterValueChangedCallback(value =>
			{
				SetLanguageGenericType(dialogueInfo.AudioClipList, (AudioClip)value.newValue);
			});
			audioClipField.SetValueWithoutNotify(GetLanguageGenericType(dialogueInfo.AudioClipList));

			// Choices.
			foreach (DialogueNodePort nodePort in dialogueInfo.DialogueNodePortList)
			{
				nodePort.TextField.RegisterValueChangedCallback(value =>
				{
					SetLanguageGenericType(nodePort.TextLanguageList, value.newValue);
				});
				nodePort.TextField.SetValueWithoutNotify(GetLanguageGenericType(nodePort.TextLanguageList));
			}
		}

		public override void LoadValueIntoField()
		{
			base.LoadValueIntoField();
			characterNamePopupField.SetValueWithoutNotify(dialogueInfo.CharacterName);
			characterPotraitPopupField.SetValueWithoutNotify(dialogueInfo.CharacterPotrait);
			potraitFacingtDirectionField.SetValueWithoutNotify(dialogueInfo.PotraitFacingDirection);
			audioClipField.SetValueWithoutNotify(GetLanguageGenericType(dialogueInfo.AudioClipList));
			textLanguageField.SetValueWithoutNotify(GetLanguageGenericType(dialogueInfo.TextLanguageList));
		}

		public Port AddChoicePort(BaseNode baseNode, DialogueNodePort dialogueNodePort = null)
		{
			Port port = NewDialogueNodePort(baseNode, dialogueNodePort);

			baseNode.outputContainer.Add(port);
			baseNode.RefreshPorts();
			baseNode.RefreshExpandedState();

			return port;
		}

		void SetLanguageGenericType<T>(List<LanguageGeneric<T>> languageGeneric, T newValue)
		{
			languageGeneric.Find(value => value.LanguageType == editorWindow.SelectedLanguage).SetLanguageGenericType(newValue);
		}

		T GetLanguageGenericType<T>(List<LanguageGeneric<T>> languageGeneric)
		{
			return (T)(object)languageGeneric.Find(value => value.LanguageType == editorWindow.SelectedLanguage).LanguageGenericType;
		}

		void CreateMainContainer()
		{
			ResoucesLoadCharacterProfile();
			CreatePotraitPreview();
			CreateTextName();
			CreatePotraitSelector();
			CreatePotraitFacingSelector();
			CreateAudioClip();
			CreateTextBox();
			CreateAddChoiceButton();
		}

		void ResoucesLoadCharacterProfile()
		{
			characterProfiles = Resources.Load<CharacterProfilesSO>(characterProfilePath);
			characterNameList = new List<string>(characterProfiles.GetCharacterNameList());
			characterSpriteList = new List<Sprite>(characterProfiles.GetCharacterSpriteList());
		}

		void CreatePotraitPreview()
		{
			// Potrait Name.
			Label labelText = new Label("Potrait");
			labelText.AddToClassList("potraitText");
			labelText.AddToClassList("Label");
			mainContainer.Add(labelText);

			// Potrait Image.
			characterPotraitPreview = new Image();
			characterPotraitPreview.AddToClassList("potraitPreview");
			mainContainer.Add(characterPotraitPreview);
		}

		void CreatePotraitSelector()
		{
			// Potrait Selector Name.
			Label labelText = new Label("Potrait Selector");
			labelText.AddToClassList("potraitSelectorText");
			labelText.AddToClassList("Label");
			mainContainer.Add(labelText);

			// Potrait Selector.
			characterPotraitPopupField = new PopupField<Sprite>(characterSpriteList, 0);
			characterPotraitPopupField.RegisterValueChangedCallback(value =>
			{
				Sprite sprite = characterProfiles.GetCharacterSprite(dialogueInfo.CharacterName, ((UnityEngine.Object)(object)value.newValue).name);
				dialogueInfo.SetCharacterPotrait(sprite);

				characterPotraitPreview.image = sprite != null ? sprite.texture : null;
			});
			characterPotraitPopupField.SetValueWithoutNotify(dialogueInfo.CharacterPotrait);
			characterPotraitPopupField.AddToClassList("Potrait");

			mainContainer.Add(characterPotraitPopupField);
		}

		void CreatePotraitFacingSelector()
		{
			// Potrait Facing Direction.
			potraitFacingtDirectionField = new EnumField() { value = dialogueInfo.PotraitFacingDirection };
			potraitFacingtDirectionField.Init(dialogueInfo.PotraitFacingDirection);
			potraitFacingtDirectionField.RegisterValueChangedCallback(value => dialogueInfo.SetPotraitFacingDirection((PotraitFacingDirection)value.newValue));
			mainContainer.Add(potraitFacingtDirectionField);
		}

		void CreateAudioClip()
		{
			// Audio Name.
			Label labelText = new Label("Audio");
			labelText.AddToClassList("audioText");
			labelText.AddToClassList("Label");
			mainContainer.Add(labelText);

			// Audio Clip.
			audioClipField = new ObjectField()
			{
				objectType = typeof(AudioClip),
				allowSceneObjects = false,
				value = dialogueInfo.AudioClipList.Find(audioClip => audioClip.LanguageType == editorWindow.SelectedLanguage).LanguageGenericType,
			};
			audioClipField.RegisterValueChangedCallback(value =>
			{
				SetLanguageGenericType(dialogueInfo.AudioClipList, (AudioClip)value.newValue);
			});
			audioClipField.SetValueWithoutNotify(GetLanguageGenericType(dialogueInfo.AudioClipList));
			mainContainer.Add(audioClipField);
		}

		void CreateTextName()
		{
			// Text Name.
			Label labelName = new Label("Name");
			labelName.AddToClassList("labelName");
			labelName.AddToClassList("Label");
			mainContainer.Add(labelName);

			characterNamePopupField = new PopupField<string>(characterNameList, 0);
			characterNamePopupField.RegisterValueChangedCallback(value =>
			{
				dialogueInfo.SetCharacterName(value.newValue);

				characterSpriteList = new List<Sprite>(characterProfiles.GetCharacterSpriteList(value.newValue));
				characterPotraitPopupField.choices = characterSpriteList;
				characterPotraitPopupField.index = 0;
			});
			characterNamePopupField.SetValueWithoutNotify(dialogueInfo.CharacterName);
			characterNamePopupField.AddToClassList("TextName");
			mainContainer.Add(characterNamePopupField);
		}

		void CreateTextBox()
		{
			// Text Box.
			Label labelText = new Label("Text Box");
			labelText.AddToClassList("labelText");
			labelText.AddToClassList("Label");
			mainContainer.Add(labelText);

			textLanguageField = new TextField("");
			textLanguageField.RegisterValueChangedCallback(value =>
			{
				SetLanguageGenericType(dialogueInfo.TextLanguageList, value.newValue);
			});
			textLanguageField.SetValueWithoutNotify(GetLanguageGenericType(dialogueInfo.TextLanguageList));
			textLanguageField.multiline = true;

			textLanguageField.AddToClassList("TextBox");
			mainContainer.Add(textLanguageField);
		}

		void CreateAddChoiceButton()
		{
			Button button = new Button() { text = "Add Choice" };
			button.clicked += () => AddChoicePort(this);

			titleButtonContainer.Add(button);
		}

		Port NewDialogueNodePort(BaseNode baseNode, DialogueNodePort dialogueNodePort = null)
		{
			int outputPortCount = baseNode.outputContainer.Query("connector").ToList().Count();
			string outputPortName = $"Choice {outputPortCount + 1}";

			Port port = GetPortInstance(Direction.Output);

			string portGuid = Guid.NewGuid().ToString();
			string inputGuid = "", outputGuid = "";
			List<LanguageGeneric<string>> textLanguageList = new List<LanguageGeneric<string>>();

			foreach (LanguageType language in Enum.GetValues(typeof(LanguageType)))
			{
				textLanguageList.Add(new LanguageGeneric<string>(language, outputPortName));
			}

			if (dialogueNodePort != null)
			{
				portGuid = dialogueNodePort.PortGuid;
				inputGuid = dialogueNodePort.InputGuid;
				outputGuid = dialogueNodePort.OutputGuid;

				foreach (LanguageGeneric<string> languageGeneric in dialogueNodePort.TextLanguageList)
				{
					textLanguageList.Find(language => language.LanguageType == languageGeneric.LanguageType).SetLanguageGenericType(languageGeneric.LanguageGenericType);
				}
			}

			// Text for the port.
			TextField textField = new TextField();
			textField.RegisterValueChangedCallback(value =>
			{
				SetLanguageGenericType(textLanguageList, value.newValue);
			});
			textField.SetValueWithoutNotify(GetLanguageGenericType(textLanguageList));

			// Delete button.
			Button deleteButton = new Button(() => DeletePort(baseNode, port)) { text = "X" };

			port.contentContainer.Add(textField);
			port.contentContainer.Add(deleteButton);

			DialogueNodePort newDialogueNodePort = new DialogueNodePort(portGuid, inputGuid, outputGuid, textField, textLanguageList);
			port.portName = newDialogueNodePort.PortGuid;

			Label portNameLabel = port.contentContainer.Q<Label>("type");
			portNameLabel.AddToClassList("PortName");

			dialogueInfo.DialogueNodePortList.Add(newDialogueNodePort);

			return port;
		}

		void DeletePort(BaseNode node, Port port)
		{
			DialogueNodePort temp = dialogueInfo.DialogueNodePortList.Find(value => value.PortGuid == port.portName);
			dialogueInfo.DialogueNodePortList.Remove(temp);

			IEnumerable<Edge> portEdge = graphView.edges.ToList().Where(edge => edge.output == port);

			if (portEdge.Any())
			{
				Edge edge = portEdge.First();
				edge.input.Disconnect(edge);
				edge.output.Disconnect(edge);
				graphView.RemoveElement(edge);
			}

			node.outputContainer.Remove(port);

			node.RefreshPorts();
			node.RefreshExpandedState();
		}
	}
}