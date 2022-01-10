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
	public class DialogueNode : BaseNodeLayout
	{
		[SerializeField] DialogueNodeData dialogueNodeData = new DialogueNodeData();

		public DialogueNodeData DialogueNodeData { get => dialogueNodeData; }

		string nodeStyleSheet = "USS/Nodes/DialogueNodeStyleSheet";

		//PopupField<string> characterNamePopupField;             // Character name popup.
		//PopupField<Sprite> characterPotraitPopupField;          // Character potrait popup.
		//EnumField potraitFacingtDirectionField;					// Potrait facing direction.

		//// Character profiles.
		//CharacterProfilesSO characterProfiles;
		//string characterProfilePath = "Character/CharacterProfiles";

		//// List of popup choices.
		//List<string> characterNameList = new List<string>();
		//List<Sprite> characterSpriteList = new List<Sprite>();

		public DialogueNode() { }

		public DialogueNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			StyleSheet styleSheet = Resources.Load<StyleSheet>(nodeStyleSheet);
			Initialize(position, editorWindow, graphView, "Dialogue", styleSheet);

			AddInputPort("Input");
			AddOutputPort("Continue");

			TopContainer();
		}

		void TopContainer()
		{
			AddPortButton();
			AddDropdownMenu();
		}

		void AddPortButton()
		{
			Button button = GetNewButton("Add Choice", () => AddChoicePort(this), null, "TopBtn");
			titleButtonContainer.Add(button);
		}

		void AddDropdownMenu()
		{
			ToolbarMenu Menu = new ToolbarMenu();
			Menu.text = "Add Content";

			Menu.menu.AppendAction("Name", new Action<DropdownMenuAction>(x => CharacterName()));
			Menu.menu.AppendAction("Image", new Action<DropdownMenuAction>(x => ShowPotrait()));
			Menu.menu.AppendAction("Text", new Action<DropdownMenuAction>(x => ShowText()));

			titleButtonContainer.Add(Menu);
		}

		public void LoadDialogueNode(DialogueNodeData node)
		{
			//LoadValueIntoField();

			//// Update the potrait preview.
			//if (node.DialogueInfo.CharacterPotrait)
			//	characterPotraitPreview.image = node.DialogueInfo.CharacterPotrait.texture;

			//// Update the popup choices.
			//List<Sprite> spriteList = characterProfiles.GetCharacterSpriteList(dialogueInfo.CharacterName);
			//if (spriteList != null)
			//	characterPotraitPopupField.choices = spriteList;
		}

		public override void LoadValueIntoField() { }

		public Port AddChoicePort(BaseNode baseNode, DialogueNodePort dialogueNodePort = null)
		{
			DialogueData_Port currentDialoguePort = new DialogueData_Port();

			// Check if we load it in with values
			if (dialogueNodePort != null)
				currentDialoguePort.SetGuid(dialogueNodePort);

			DialogueNodeData.PortList.Add(currentDialoguePort);

			Port port = GetPortInstance(Direction.Output);
			port.portName = currentDialoguePort.PortGuid;                       // We use portName as port ID
			port.portColor = Color.yellow;

			Button deleteButton = GetDeleteButton(null, null, () => DeletePort(baseNode, port));
			port.contentContainer.Add(deleteButton);

			Label portNameLabel = port.contentContainer.Q<Label>("type");       // Get Labal in port that is used to contain the port name.
			portNameLabel.AddToClassList("PortName");                           // Here we add a uss class to it so we can hide it in the editor window.

			baseNode.outputContainer.Add(port);

			// Refresh
			baseNode.RefreshPorts();
			baseNode.RefreshExpandedState();

			return port;
		}

		public void ShowText(DialogueData_Text dataText = null)
		{
			DialogueData_Text currentDialogueText = new DialogueData_Text();
			dialogueNodeData.BaseContainerList.Add(currentDialogueText);

			// Add Container Box
			Box boxContainer = new Box();
			boxContainer.AddToClassList("DialogueBox");

			Action<Box> visualElementAct = (topBoxContainer) =>
			{
				GetNewLabel("Text", topBoxContainer, "LabelText", "TextColor");
				GetNewTextField_AudioClipLanguage(currentDialogueText.AudioClipList, topBoxContainer, "AudioClip");
			};
			Box topBoxContainer = GetBox(currentDialogueText, visualElementAct, boxContainer);
			boxContainer.Add(topBoxContainer);

			GetNewTextField_TextLanguage(currentDialogueText.TextList, "Text area..", boxContainer, "TextBox");
			mainContainer.Add(boxContainer);

			LoadInTextLine(currentDialogueText, dataText);
			ReloadLanguage();
		}

		public void ShowPotrait(DialogueData_Images dataImages = null)
		{
			DialogueData_Images currentDialogueImage = new DialogueData_Images();

			if (dataImages != null)
				currentDialogueImage.SetSpriteSprites(dataImages.SpriteLeft.Value, dataImages.SpriteRight.Value);

			dialogueNodeData.BaseContainerList.Add(currentDialogueImage);

			Box boxContainer = new Box();
			boxContainer.AddToClassList("DialogueBox");

			Action<Box> visualElementAct = (topBoxContainer) =>
			{
				GetNewLabel("Potrait", topBoxContainer, "LabelText", "TextColor");
			};
			Box topBoxContainer = GetBox(currentDialogueImage, visualElementAct, boxContainer);

			boxContainer.Add(topBoxContainer);
			AddImages(currentDialogueImage, boxContainer);

			mainContainer.Add(boxContainer);
		}

		public void CharacterName(DialogueData_Name dataName = null)
		{
			DialogueData_Name currentDialogueName = new DialogueData_Name();

			if (dataName != null)
				currentDialogueName.SetCharacterName(dataName.CharacterName);

			dialogueNodeData.BaseContainerList.Add(currentDialogueName);

			Box boxContainer = new Box();
			boxContainer.AddToClassList("CharacterBox");

			Action<Box> visualElementAct = (topBoxContainer) =>
			{
				GetNewLabel("Name", topBoxContainer, "LabelText", "NameColor");
				GetNewTextField(currentDialogueName.CharacterName, "Name", topBoxContainer, "CharacterName");
			};
			Box topBoxContainer = GetBox(currentDialogueName, visualElementAct, boxContainer);

			boxContainer.Add(topBoxContainer);
			mainContainer.Add(boxContainer);
		}

		Box GetBox(DialogueData_BaseContainer container, Action<Box> visualElementAct, Box deleteButtonContainer)
		{
			Action onDeleteAct = () => { dialogueNodeData.BaseContainerList.Remove(container); };
			return AddBaseBoxContainer(visualElementAct, deleteButtonContainer, onDeleteAct, "TopBox");
		}

		void LoadInTextLine(DialogueData_Text currentDialogueText, DialogueData_Text data_Text)
		{
			if (data_Text == null) return;

			// Guid ID
			currentDialogueText.SetGuid(data_Text.GuidID);

			// Text
			foreach (LanguageGeneric<string> data_text in data_Text.TextList)
			{
				foreach (LanguageGeneric<string> text in currentDialogueText.TextList)
				{
					if (text.LanguageType == data_text.LanguageType)
					{
						text.SetLanguageGenericType(data_text.LanguageGenericType);
						break;
					}
				}
			}

			// Audio
			foreach (LanguageGeneric<AudioClip> data_audioclip in data_Text.AudioClipList)
			{
				foreach (LanguageGeneric<AudioClip> audioclip in currentDialogueText.AudioClipList)
				{
					if (audioclip.LanguageType == data_audioclip.LanguageType)
					{
						audioclip.SetLanguageGenericType(data_audioclip.LanguageGenericType);
						break;
					}
				}
			}
		}

		void AddImages(DialogueData_Images container, Box boxContainer)
		{
			Box ImagePreviewBox = new Box();
			Box ImagesBox = new Box();

			ImagePreviewBox.AddToClassList("BoxRow");
			ImagesBox.AddToClassList("BoxRow");

			// Set up Image Preview.
			Image leftImage = GetNewImage(ImagePreviewBox, "ImagePreview", "ImagePreviewLeft");
			Image rightImage = GetNewImage(ImagePreviewBox, "ImagePreview", "ImagePreviewRight");

			// Set up Sprite.
			GetNewObjectField_Sprite(container.SpriteLeft, leftImage, ImagesBox, "SpriteLeft");
			GetNewObjectField_Sprite(container.SpriteRight, rightImage, ImagesBox, "SpriteRight");

			// Add to box container.
			boxContainer.Add(ImagePreviewBox);
			boxContainer.Add(ImagesBox);
		}

		void ResoucesLoadCharacterProfile()
		{
			//characterProfiles = Resources.Load<CharacterProfilesSO>(characterProfilePath);
			//characterNameList = new List<string>(characterProfiles.GetCharacterNameList());
			//characterSpriteList = new List<Sprite>(characterProfiles.GetCharacterSpriteList());
		}

		void CreatePotraitPreview()
		{
			//// Potrait Name.
			//Label labelText = new Label("Potrait");
			//labelText.AddToClassList("potraitText");
			//labelText.AddToClassList("Label");
			//mainContainer.Add(labelText);

			//// Potrait Image.
			//characterPotraitPreview = new Image();
			//characterPotraitPreview.AddToClassList("potraitPreview");
			//mainContainer.Add(characterPotraitPreview);
		}

		void CreatePotraitSelector()
		{
			//// Potrait Selector Name.
			//Label labelText = new Label("Potrait Selector");
			//labelText.AddToClassList("potraitSelectorText");
			//labelText.AddToClassList("Label");
			//mainContainer.Add(labelText);

			//// Potrait Selector.
			//characterPotraitPopupField = new PopupField<Sprite>(characterSpriteList, 0);
			//characterPotraitPopupField.RegisterValueChangedCallback(value =>
			//{
			//	Sprite sprite = characterProfiles.GetCharacterSprite(dialogueInfo.CharacterName, ((UnityEngine.Object)(object)value.newValue).name);
			//	dialogueInfo.SetCharacterPotrait(sprite);

			//	characterPotraitPreview.image = sprite != null ? sprite.texture : null;
			//});
			//characterPotraitPopupField.SetValueWithoutNotify(dialogueInfo.CharacterPotrait);
			//characterPotraitPopupField.AddToClassList("Potrait");

			//mainContainer.Add(characterPotraitPopupField);
		}

		void CreatePotraitFacingSelector()
		{
			//// Potrait Facing Direction.
			//potraitFacingtDirectionField = new EnumField() { value = dialogueInfo.PotraitFacingDirection };
			//potraitFacingtDirectionField.Init(dialogueInfo.PotraitFacingDirection);
			//potraitFacingtDirectionField.RegisterValueChangedCallback(value => dialogueInfo.SetPotraitFacingDirection((PotraitFacingDirection)value.newValue));
			//mainContainer.Add(potraitFacingtDirectionField);
		}

		void DeletePort(BaseNode node, Port port)
		{
			DialogueData_Port tmp = dialogueNodeData.PortList.Find(findPort => findPort.PortGuid == port.portName);
			dialogueNodeData.PortList.Remove(tmp);

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