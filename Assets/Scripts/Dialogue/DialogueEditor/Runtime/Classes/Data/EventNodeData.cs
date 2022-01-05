using System;
using UnityEngine;

using DialogueEditor.Runtime.ScriptableObjects;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class EventNodeData : BaseNodeData
	{
		[SerializeField] DialogueEventSO dialogueEventSO;

		public DialogueEventSO DialogueEventSO { get => dialogueEventSO; }

		public EventNodeData(DialogueEventSO dialogueEventSO)
		{
			this.dialogueEventSO = dialogueEventSO;
		}
	}
}