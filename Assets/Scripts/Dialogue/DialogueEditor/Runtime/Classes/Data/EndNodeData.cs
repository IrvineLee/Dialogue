using System;
using UnityEngine;

using DialogueEditor.Runtime.Enums.Nodes;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class EndNodeData : BaseNodeData
	{
		[SerializeField] EndNodeType endNodeType;

		public EndNodeType EndNodeType { get => endNodeType; }

		public EndNodeData(EndNodeType endNodeType)
		{
			this.endNodeType = endNodeType;
		}
	}
}