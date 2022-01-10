using System;
using UnityEngine;

using DialogueEditor.Runtime.ScriptableObjects;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class Container_DialogueEventSO : Container<DialogueEventSO>
	{
		public Container_DialogueEventSO() { }

		public Container_DialogueEventSO(DialogueEventSO dialogueEvent) { SetValue(dialogueEvent); }

		public void SetModifier(DialogueEventSO dialogueEvent) { SetValue(dialogueEvent); }
	}
}