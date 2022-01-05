using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DialogueEditor.Runtime.Enums.Nodes;
using UnityEngine.Events;

namespace DialogueDisplay
{
	public class DialogueController : MonoBehaviour
	{
		[SerializeField] GameObject dialogueUI;

		[Header("Character")]
		[SerializeField] Transform potraitParent;
		[SerializeField] DialogueBox dialogueBox;
		[SerializeField] Transform selectionButtonParent;

		void Awake()
		{
			ShowDialogue(false);
		}

		public void ShowDialogue(bool isShow)
		{
			dialogueUI?.SetActive(isShow);
		}

		public void SetText(string name, string text)
		{
			dialogueBox?.SetText(name, text);
		}

		public void SetImage(Sprite sprite, PotraitFaceImageDirection dialogueFaceImageDirection)
		{
			// TODO: Potrait setting
			if (sprite)
			{
				if (dialogueFaceImageDirection == PotraitFaceImageDirection.Left)
				{
					GameObject leftImageGO = potraitParent.GetChild(0).GetChild(0).gameObject;
					leftImageGO.GetComponent<Image>().sprite = sprite;
				}
				else if (dialogueFaceImageDirection == PotraitFaceImageDirection.Right)
				{
					GameObject rightImageGO = potraitParent.GetChild(0).GetChild(1).gameObject;
					rightImageGO.GetComponent<Image>().sprite = sprite;
				}
			}
		}

		public void SetButtons(List<string> textList, List<UnityAction> unityActionList)
		{
			// TODO: Button spawning
			List<DialogueButton> dialogueButtonList = new List<DialogueButton>();
			foreach (Transform child in selectionButtonParent)
			{
				DialogueButton dialogueButton = child.GetComponent<DialogueButton>();
				dialogueButtonList.Add(dialogueButton);

				child.gameObject.SetActive(false);
			}

			for (int i = 0; i < textList.Count; i++)
			{
				dialogueButtonList[i].SetButton(textList[i], unityActionList[i]);
			}
		}
	}
}