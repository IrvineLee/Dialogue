using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor.Runtime.Classes.Data;
using DialogueEditor.Runtime.Classes;
using UnityEngine.Events;
using DialogueEditor.Runtime.Enums.Nodes;

namespace DialogueDisplay
{
	public class TestDialogue : MonoBehaviour
	{
		DialogueTalk dialogueTalk;

		void Awake()
		{
			dialogueTalk = GetComponent<DialogueTalk>();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				dialogueTalk.StartDialogue();
			}
		}
	}
}