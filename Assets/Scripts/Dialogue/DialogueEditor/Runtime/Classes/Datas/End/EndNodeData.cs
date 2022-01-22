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

		public EndNodeData(EnumContainer<EndNodeType> endNodeType)
		{
			SetEndNodeData(endNodeType);
		}

		public void SetEndNodeData(EnumContainer<EndNodeType> endNodeType)
		{
			this.endNodeType.SetValue(endNodeType.Value);
		}
	}
}