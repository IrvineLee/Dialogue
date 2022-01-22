using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

using DialogueEditor.Editor.GraphView;
using DialogueEditor.Runtime.Classes;
using DialogueEditor.Runtime.Classes.Data;
using Character.ScriptableObjects;
using Dico.Helper;
using DialogueEditor.Runtime;

namespace DialogueEditor.Editor.Nodes
{
	public class DialogueNode : BaseNodeLayout
	{
		[SerializeField] DialogueNodeData dialogueNodeData = new DialogueNodeData();

		public DialogueNodeData DialogueNodeData { get => dialogueNodeData; }

		string nodeStyleSheet = "USS/Nodes/DialogueNodeStyleSheet";

		CharacterProfilesSO characterProfile = null;
		//List<(string, Color)> characterNameList;
		//CharacterProfilesSO characterSpriteList;
		string characterProfilePath = "Character/CharacterProfiles";

		public DialogueNode() { }

		public DialogueNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			StyleSheet styleSheet = Resources.Load<StyleSheet>(nodeStyleSheet);
			Initialize(position, editorWindow, graphView, "Dialogue", styleSheet);

			AddInputPort("Input");
			AddOutputPort("Continue");

			TopContainer();
			ResoucesLoadCharacterProfile();
		}

		public void LoadDialogueNode(DialogueNodeData node)
		{
			SetNodeGuid(node.NodeGuid);

			List<DialogueData_BaseContainer> data_BaseContainer = new List<DialogueData_BaseContainer>();

			data_BaseContainer.AddRange(node.DialogueInfo.NameList);
			data_BaseContainer.AddRange(node.DialogueInfo.ImagesList);
			data_BaseContainer.AddRange(node.DialogueInfo.TextList);

			data_BaseContainer.Sort(delegate (DialogueData_BaseContainer x, DialogueData_BaseContainer y)
			{
				return x.Id.Value.CompareTo(y.Id.Value);
			});

			foreach (DialogueData_BaseContainer data in data_BaseContainer)
			{
				switch (data)
				{
					case DialogueData_Name Name:
						CharacterName(Name);
						break;
					case DialogueData_Text Text:
						ShowText(Text);
						break;
					case DialogueData_Images image:
						ShowPotrait(image);
						break;
					default:
						break;
				}
			}

			foreach (DialogueData_Port port in node.PortList)
			{
				AddChoicePort(this, port);
			}
		}

		public override void LoadValueIntoField() { }

		void TopContainer()
		{
			AddPortButton();
			AddDropdownMenu();
		}

		void ResoucesLoadCharacterProfile()
		{
			characterProfile = Resources.Load<CharacterProfilesSO>(characterProfilePath);
			//characterNameList = new List<(string, Color)>(characterProfile.GetCharacterNameList());
			//characterSpriteList = new List<Sprite>(characterProfile.GetCharacterSpriteList());
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

		Port AddChoicePort(BaseNode baseNode, DialogueData_Port dialogueDataPort = null)
		{
			DialogueData_Port currentDialoguePort = new DialogueData_Port();

			// Check if we load it in with values
			if (dialogueDataPort != null)
				currentDialoguePort.SetGuid(dialogueDataPort);

			dialogueNodeData.PortList.Add(currentDialoguePort);

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

		void ShowText(DialogueData_Text dataText = null)
		{
			DialogueData_Text currentDialogueText = new DialogueData_Text();
			dialogueNodeData.DialogueInfo.BaseContainerList.Add(currentDialogueText);

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

		void ShowPotrait(DialogueData_Images dataImages = null)
		{
			DialogueData_Images currentDialogueImage = new DialogueData_Images();

			if (dataImages != null)
				currentDialogueImage.SetValues(dataImages);

			dialogueNodeData.DialogueInfo.BaseContainerList.Add(currentDialogueImage);

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

		void CharacterName(DialogueData_Name dataName = null)
		{
			DialogueData_Name currentDialogueName = new DialogueData_Name();

			if (dataName != null)
				currentDialogueName.SetCharacterName(dataName.CharacterName.Value);

			dialogueNodeData.DialogueInfo.BaseContainerList.Add(currentDialogueName);

			Box boxContainer = new Box();
			boxContainer.AddToClassList("CharacterBox");

			Action<Box> visualElementAct = (topBoxContainer) =>
			{
				GetNewLabel("Name", topBoxContainer, "LabelText", "NameColor");
				GetNewPopupField(currentDialogueName.CharacterName, characterProfile.GetCharacterNameList(), topBoxContainer, "NamePopup");
				//GetNewTextField(currentDialogueName.CharacterName, "Name", topBoxContainer, "CharacterName");
			};
			Box topBoxContainer = GetBox(currentDialogueName, visualElementAct, boxContainer);

			boxContainer.Add(topBoxContainer);
			mainContainer.Add(boxContainer);
		}

		Box GetBox(DialogueData_BaseContainer container, Action<Box> visualElementAct, Box deleteButtonContainer)
		{
			Action onDeleteAct = () => { dialogueNodeData.DialogueInfo.BaseContainerList.Remove(container); };
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
			StringContainer stringContainer = new StringContainer(characterProfile.CharacterProfileList[0].CharacterName);
			GetNewPopupField(stringContainer, characterProfile.GetCharacterNameList(), ImagesBox, "NamePopupImage");

			GetNewObjectField_Sprite(container.SpriteLeft, leftImage, ImagesBox, "SpriteLeft");
			GetNewObjectField_Sprite(container.SpriteRight, rightImage, ImagesBox, "SpriteRight");

			// Add to box container.
			boxContainer.Add(ImagePreviewBox);
			boxContainer.Add(ImagesBox);
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