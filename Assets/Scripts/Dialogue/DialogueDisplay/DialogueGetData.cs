using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor.Runtime.ScriptableObjects;
using DialogueEditor.Runtime.Classes.Data;
using DialogueEditor.Runtime.Classes;

namespace DialogueDisplay
{
	public class DialogueGetData : MonoBehaviour
	{
		[SerializeField] DialogueContainerSO dialogueContainerSO = null;

		public DialogueContainerSO DialogueContainerSO { get => dialogueContainerSO; }

		protected BaseNodeData GetNodeByGuid(string targetNodeGuid)
		{
			return dialogueContainerSO.AllNodes.Find(node => node.NodeGuid == targetNodeGuid);
		}

		protected BaseNodeData GetNodeByNodePort(DialogueNodePort nodePort)
		{
			return dialogueContainerSO.AllNodes.Find(node => node.NodeGuid == nodePort.InputGuid);
		}

		protected BaseNodeData GetNextNode(BaseNodeData baseNodeData)
		{
			NodeLinkData nodeLinkData = dialogueContainerSO.NodeLinkDataList.Find(edge => edge.BaseNodeGuid == baseNodeData.NodeGuid);
			return GetNodeByGuid(nodeLinkData.TargetNodeGuid);
		}
	}
}