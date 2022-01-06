using System;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor.Runtime.ScriptableObjects;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class EventNodeData : BaseNodeData
	{
		[SerializeField] List<EventStringIDData> eventStringIDDataList = new List<EventStringIDData>();
		[SerializeField] List<EventScriptableObjectData> eventScriptableObjectDataList = new List<EventScriptableObjectData>();

		public List<EventStringIDData> EventStringIDDataList { get => eventStringIDDataList; }
		public List<EventScriptableObjectData> EventScriptableObjectDataList { get => eventScriptableObjectDataList; }

		public EventNodeData(List<EventStringIDData> eventStringIDDataList, List<EventScriptableObjectData> eventScriptableObjectDataList)
		{
			this.eventStringIDDataList = eventStringIDDataList;
			this.eventScriptableObjectDataList = eventScriptableObjectDataList;
		}
	}
}