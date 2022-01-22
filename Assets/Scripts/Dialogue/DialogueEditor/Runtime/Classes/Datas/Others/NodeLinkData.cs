using System;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class NodeLinkData
	{
		[SerializeField] string baseNodeGuid;
		[SerializeField] string basePortName;
		[SerializeField] string targetNodeGuid;
		[SerializeField] string targetPortName;

		public string BaseNodeGuid { get => baseNodeGuid; }
		public string BasePortName { get => basePortName; }
		public string TargetNodeGuid { get => targetNodeGuid; }
		public string TargetPortName { get => targetPortName; }

		public NodeLinkData(string baseNodeGuid, string basePortName, string targetNodeGuid, string targetPortName)
		{
			this.baseNodeGuid = baseNodeGuid;
			this.basePortName = basePortName;
			this.targetNodeGuid = targetNodeGuid;
			this.targetPortName = targetPortName;
		}
	}
}