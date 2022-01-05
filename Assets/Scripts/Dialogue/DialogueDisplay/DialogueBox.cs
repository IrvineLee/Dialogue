using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DialogueDisplay
{
	public class DialogueBox : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI characterName = null;
		[SerializeField] TextMeshProUGUI characterDialogue = null;

		public TextMeshProUGUI CharacterName { get => characterName; }
		public TextMeshProUGUI CharacterDialogue { get => characterDialogue; }

		public void SetText(string name, string text)
		{
			characterName.text = name;
			characterDialogue.text = text;
		}
	}
}