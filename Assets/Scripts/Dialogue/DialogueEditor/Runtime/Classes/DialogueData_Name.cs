using System;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes
{
	[Serializable]
	public class DialogueData_Name : DialogueData_BaseContainer
	{
		[SerializeField] StringContainer characterName = new StringContainer();

		public StringContainer CharacterName { get => characterName; }

		public DialogueData_Name() { }

		public DialogueData_Name(string characterName)
		{
			this.characterName = new StringContainer(characterName);
		}

		public void SetCharacterName(StringContainer characterName) { this.characterName = characterName; }
	}
}