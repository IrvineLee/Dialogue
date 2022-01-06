using System;
using UnityEngine;

using DialogueEditor.Runtime.ScriptableObjects;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class EventScriptableObjectData
	{
		[SerializeField] DialogueEventSO dialogueEventSO;

		public DialogueEventSO DialogueEventSO { get => dialogueEventSO; }

		public EventScriptableObjectData() { }

		public EventScriptableObjectData(DialogueEventSO dialogueEventSO)
		{
			this.dialogueEventSO = dialogueEventSO;
		}

		public void SetDialogueEventSO(DialogueEventSO dialogueEventSO) { this.dialogueEventSO = dialogueEventSO; }
	}
}