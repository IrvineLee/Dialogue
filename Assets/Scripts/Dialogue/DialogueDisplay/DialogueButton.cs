using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DialogueDisplay
{
	public class DialogueButton : MonoBehaviour
	{
		[SerializeField] Button button = null;
		[SerializeField] TextMeshProUGUI text = null;

		public Button Button { get => button; }
		public TextMeshProUGUI Text { get => text; }

		public void SetButton(string text, UnityAction unityAction)
		{
			this.text.text = text;

			button.gameObject.SetActive(true);
			button.onClick = new Button.ButtonClickedEvent();
			button.onClick.AddListener(unityAction);
		}
	}
}