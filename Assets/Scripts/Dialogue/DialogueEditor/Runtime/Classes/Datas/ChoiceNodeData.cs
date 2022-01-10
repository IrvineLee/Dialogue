using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

using DialogueEditor.Runtime.Enums.Nodes;


namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class ChoiceNodeData : BaseNodeData
	{
#if UNITY_EDITOR
		[SerializeField] TextField textField;
		[SerializeField] ObjectField objectField;
#endif

		[SerializeField] EnumContainer<ChoiceStateType> choiceStateType = new EnumContainer<ChoiceStateType>();
		[SerializeField] List<LanguageGeneric<string>> textList = new List<LanguageGeneric<string>>();
		[SerializeField] List<LanguageGeneric<AudioClip>> audioClipList = new List<LanguageGeneric<AudioClip>>();
		[SerializeField] List<EventData_StringCondition> eventData_StringConditionList = new List<EventData_StringCondition>();

		public TextField TextField { get => textField; }
		public ObjectField ObjectField { get => objectField; }
		public EnumContainer<ChoiceStateType> ChoiceStateType { get => choiceStateType; }
		public List<LanguageGeneric<string>> TextList { get => textList; }
		public List<LanguageGeneric<AudioClip>> AudioClipList { get => audioClipList; }
		public List<EventData_StringCondition> EventData_StringConditionList { get => eventData_StringConditionList; }

		public void SetTextField(TextField textField) { this.textField = textField; }
		public void SetObjectField(ObjectField objectField) { this.objectField = objectField; }

		//public NodeLinkData(string baseNodeGuid, string basePortName, string targetNodeGuid, string targetPortName)
		//{
		//	this.baseNodeGuid = baseNodeGuid;
		//	this.basePortName = basePortName;
		//	this.targetNodeGuid = targetNodeGuid;
		//	this.targetPortName = targetPortName;
		//}
	}
}