using System;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class NodeLinkData
	{
		[SerializeField] string baseNodeGuid;
		[SerializeField] string targetNodeGuid;

		public string BaseNodeGuid { get => baseNodeGuid; }
		public string TargetNodeGuid { get => targetNodeGuid; }

		public NodeLinkData(string baseNodeGuid, string targetNodeGuid)
		{
			this.baseNodeGuid = baseNodeGuid;
			this.targetNodeGuid = targetNodeGuid;
		}
	}
}