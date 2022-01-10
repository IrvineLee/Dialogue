using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class EventNodeData : BaseNodeData
	{
		[SerializeField] List<EventData_StringModifier> eventData_StringModifierList = new List<EventData_StringModifier>();
		[SerializeField] List<Container_DialogueEventSO> container_DialogueEventSOList = new List<Container_DialogueEventSO>();

		public List<EventData_StringModifier> EventData_StringModifierList { get => eventData_StringModifierList; }
		public List<Container_DialogueEventSO> Container_DialogueEventSOList { get => container_DialogueEventSOList; }
	}
}