using System;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class BaseNodeData
	{
		[SerializeField] protected string nodeGuid;
		[SerializeField] protected Vector2 position;

		public string NodeGuid { get => nodeGuid; }
		public Vector2 Position { get => position; }

		public virtual void SetNode(string nodeGuid, Vector2 position)
		{
			this.nodeGuid = nodeGuid;
			this.position = position;
		}
	}
}