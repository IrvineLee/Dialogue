using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

namespace DialogueEditor.Runtime.Classes
{
	[Serializable]
	public class DialogueData_Text : DialogueData_BaseContainer
	{
#if UNITY_EDITOR
		[SerializeField] TextField textField = new TextField();
		[SerializeField] ObjectField objectField = new ObjectField();
#endif

		[SerializeField] StringContainer guidID = new StringContainer();
		[SerializeField] List<LanguageGeneric<string>> textList = new List<LanguageGeneric<string>>();
		[SerializeField] List<LanguageGeneric<AudioClip>> audioClipList = new List<LanguageGeneric<AudioClip>>();

		public DialogueData_Text()
		{
			guidID.SetValue(Guid.NewGuid().ToString());
		}

		public StringContainer GuidID { get => guidID; }
		public List<LanguageGeneric<string>> TextList { get => textList; }
		public List<LanguageGeneric<AudioClip>> AudioClipList { get => audioClipList; }

		public void SetGuid(StringContainer guidID) { this.guidID.SetValue(guidID.Value); }

		public void SetTextList(List<LanguageGeneric<string>> textList) { this.textList = textList; }

		public void SetAudioList(List<LanguageGeneric<AudioClip>> audioClipList) { this.audioClipList = audioClipList; }

		public void UpdateLanguage(List<LanguageGeneric<string>> textList, List<LanguageGeneric<AudioClip>> audioClipList)
		{
			SetTextList(textList);
			SetAudioList(audioClipList);
		}
	}
}