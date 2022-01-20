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

		public EventNodeData() { }
		public EventNodeData(List<EventData_StringModifier> stringModifierList, List<Container_DialogueEventSO> dialogueEventSOList)
		{
			// Save String Event
			foreach (EventData_StringModifier stringEvent in stringModifierList)
			{
				EventData_StringModifier tmp = new EventData_StringModifier();
				tmp.SetModifier(stringEvent);

				eventData_StringModifierList.Add(tmp);
			}

			// Save Dialogue Event
			foreach (Container_DialogueEventSO dialogueEvent in dialogueEventSOList)
			{
				container_DialogueEventSOList.Add(dialogueEvent);
			}
		}
	}
}