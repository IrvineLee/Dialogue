using UnityEngine;
using System;
using System.Collections.Generic;

using DialogueEditor.Runtime.Classes.Data;

namespace DialogueEditor.Runtime.ScriptableObjects
{
	[CreateAssetMenu(fileName = "DialogueContainer", menuName = "Dialogue/New Dialogue")]
	[Serializable]
	public class DialogueContainerSO : ScriptableObject
	{
		[SerializeField] List<NodeLinkData> nodeLinkDataList = new List<NodeLinkData>();

		[SerializeField] List<StartNodeData> startNodeDataList = new List<StartNodeData>();
		[SerializeField] List<DialogueNodeData> dialogueNodeDataList = new List<DialogueNodeData>();
		[SerializeField] List<EventNodeData> eventNodeDataList = new List<EventNodeData>();
		[SerializeField] List<EndNodeData> endNodeDataList = new List<EndNodeData>();

		public List<NodeLinkData> NodeLinkDataList { get => nodeLinkDataList; }
		public List<StartNodeData> StartNodeDataList { get => startNodeDataList; }
		public List<DialogueNodeData> DialogueNodeDataList { get => dialogueNodeDataList; }
		public List<EventNodeData> EventNodeDataList { get => eventNodeDataList; }
		public List<EndNodeData> EndNodeDataList { get => endNodeDataList; }

		public List<BaseNodeData> AllNodes
		{
			get
			{
				List<BaseNodeData> baseNodeDataList = new List<BaseNodeData>();
				baseNodeDataList.AddRange(StartNodeDataList);
				baseNodeDataList.AddRange(DialogueNodeDataList);
				baseNodeDataList.AddRange(EventNodeDataList);
				baseNodeDataList.AddRange(EndNodeDataList);

				return baseNodeDataList;
			}
		}
	}
}