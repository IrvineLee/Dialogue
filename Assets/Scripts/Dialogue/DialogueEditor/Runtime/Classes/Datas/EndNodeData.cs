using System;
using UnityEngine;

using DialogueEditor.Runtime.Enums.Nodes;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class EndNodeData : BaseNodeData
	{
		[SerializeField] EnumContainer<EndNodeType> endNodeType = new EnumContainer<EndNodeType>();

		public EnumContainer<EndNodeType> EndNodeType { get => endNodeType; }

		public EndNodeData() { }

		public EndNodeData(EndNodeType endNodeType)
		{
			this.endNodeType = new EnumContainer<EndNodeType>(endNodeType);
		}
	}
}