using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class BranchNodeData : BaseNodeData
	{
		[SerializeField] string trueGuidNode;
		[SerializeField] string falseGuidNode;
		[SerializeField] List<EventData_StringCondition> eventData_StringConditionList = new List<EventData_StringCondition>();

		public string TrueGuidNode { get => trueGuidNode; }
		public string FalseGuidNode { get => falseGuidNode; }
		public List<EventData_StringCondition> EventData_StringConditionList { get => eventData_StringConditionList; }

		public BranchNodeData() { }

		public BranchNodeData(string trueGuidNode, string falseGuidNode, List<EventData_StringCondition> eventData_StringConditionList)
		{
			this.trueGuidNode = trueGuidNode;
			this.falseGuidNode = falseGuidNode;
			this.eventData_StringConditionList = eventData_StringConditionList;

			foreach (EventData_StringCondition stringEvent in eventData_StringConditionList)
			{
				EventData_StringCondition tempStrCondition = new EventData_StringCondition();
				tempStrCondition.SetModifier(stringEvent);

				eventData_StringConditionList.Add(tempStrCondition);
			}
		}
	}
}