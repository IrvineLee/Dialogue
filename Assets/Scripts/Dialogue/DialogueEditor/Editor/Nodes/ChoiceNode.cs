using System;
using UnityEngine;
using UnityEngine.UIElements;

using DialogueEditor.Editor.GraphView;
using DialogueEditor.Runtime.Classes.Data;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using DialogueEditor.Runtime.Classes;

namespace DialogueEditor.Editor.Nodes
{
	public class ChoiceNode : BaseNodeLayout
	{
		[SerializeField] ChoiceNodeData choiceNodeData = new ChoiceNodeData();

		string nodeStyleSheet = "USS/Nodes/ChoiceNodeStyleSheet";

		public ChoiceNodeData ChoiceNodeData { get => choiceNodeData; }

		Box choiceStateEnumBox;

		public ChoiceNode() { }

		public ChoiceNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			StyleSheet styleSheet = Resources.Load<StyleSheet>(nodeStyleSheet);
			Initialize(position, editorWindow, graphView, "Choice", styleSheet);

			nodeGuid = Guid.NewGuid().ToString();

			Port inputPort = AddInputPort("Input");
			AddOutputPort("Output");

			inputPort.portColor = Color.yellow;

			TopButton();

			TextLine();
			ChoiceStateEnum();
		}

		public override void LoadValueIntoField()
		{
			if (choiceNodeData.ChoiceStateType.EnumField != null)
				choiceNodeData.ChoiceStateType.EnumField.SetValueWithoutNotify(choiceNodeData.ChoiceStateType.Value);
		}
		protected override void OnDeleteBox(Box boxContainer)
		{
			base.OnDeleteBox(boxContainer);
			ShowHideChoiceEnum();
		}

		public void LoadChoiceNode(ChoiceNodeData node)
		{
			SetNodeGuid(node.NodeGuid);
			choiceNodeData.ChoiceStateType.SetValue(node.ChoiceStateType.Value);

			foreach (LanguageGeneric<string> dataText in node.TextList)
			{
				foreach (LanguageGeneric<string> editorText in choiceNodeData.TextList)
				{
					if (editorText.LanguageType == dataText.LanguageType)
						editorText.SetLanguageGenericType(dataText.LanguageGenericType);
				}
			}

			foreach (LanguageGeneric<AudioClip> dataAudioClip in node.AudioClipList)
			{
				foreach (LanguageGeneric<AudioClip> editorAudioClip in choiceNodeData.AudioClipList)
				{
					if (editorAudioClip.LanguageType == dataAudioClip.LanguageType)
						editorAudioClip.SetLanguageGenericType(dataAudioClip.LanguageGenericType);
				}
			}

			foreach (EventData_StringCondition item in node.EventData_StringConditionList)
			{
				AddCondition(item);
			}

			LoadValueIntoField();
			ReloadLanguage();
		}

		void TopButton()
		{
			ToolbarMenu Menu = new ToolbarMenu();
			Menu.text = "Add Condition";

			Menu.menu.AppendAction("String Event Condition", new Action<DropdownMenuAction>(x => AddCondition()));

			titleButtonContainer.Add(Menu);
		}

		void AddCondition(EventData_StringCondition stringEvent = null)
		{
			AddStringConditionEventBuild(choiceNodeData.EventData_StringConditionList, stringEvent);
			ShowHideChoiceEnum();
		}

		void TextLine()
		{
			// Make Container Box
			Box boxContainer = new Box();
			boxContainer.AddToClassList("TextLineBox");

			// Text
			TextField textField = GetNewTextField_TextLanguage(choiceNodeData.TextList, "Text", boxContainer, "TextBox");
			choiceNodeData.SetTextField(textField);

			// Audio
			ObjectField objectField = GetNewTextField_AudioClipLanguage(choiceNodeData.AudioClipList, boxContainer, "AudioClip");
			choiceNodeData.SetObjectField(objectField);
			
			ReloadLanguage();
			mainContainer.Add(boxContainer);
		}

		void ChoiceStateEnum()
		{
			choiceStateEnumBox = new Box();
			choiceStateEnumBox.AddToClassList("BoxRow");
			ShowHideChoiceEnum();

			// Make fields.
			GetNewLabel("If the condition is not met", choiceStateEnumBox, "ChoiceLabel");
			GetNewEnumField_ChoiceStateType(choiceNodeData.ChoiceStateType, choiceStateEnumBox, "enumHide");

			mainContainer.Add(choiceStateEnumBox);
		}

		void ShowHideChoiceEnum()
		{
			ShowHide(choiceNodeData.EventData_StringConditionList.Count > 0, choiceStateEnumBox);
		}
	}
}