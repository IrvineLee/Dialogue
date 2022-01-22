using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

using Dico.Helper;

namespace DialogueEditor.Runtime.Classes
{
	[Serializable]
	public class DialogueData_Text : DialogueData_BaseContainer
	{
#if UNITY_EDITOR
		[SerializeField] TextField textField;
		[SerializeField] ObjectField objectField;
#endif

		[SerializeField] StringContainer guidID = new StringContainer();
		[SerializeField] List<LanguageGeneric<string>> textList = new List<LanguageGeneric<string>>();
		[SerializeField] List<LanguageGeneric<AudioClip>> audioClipList = new List<LanguageGeneric<AudioClip>>();

		public DialogueData_Text()
		{
			guidID.SetValue(Guid.NewGuid().ToString());
		}

		public string GuidID { get => guidID.Value; }
		public List<LanguageGeneric<string>> TextList { get => textList; }
		public List<LanguageGeneric<AudioClip>> AudioClipList { get => audioClipList; }

		public void SetGuid(string guidID) { this.guidID.SetValue(guidID); }

		public void SetTextList(List<LanguageGeneric<string>> textList) { this.textList = ObjectHelper.Clone(textList); }

		public void SetAudioList(List<LanguageGeneric<AudioClip>> audioClipList) 
		{
			this.audioClipList = audioClipList.Select(audio => new LanguageGeneric<AudioClip>(audio.LanguageType, audio.LanguageGenericType)).ToList();
		}
		
		public void SetValues(DialogueData_Text dialogueData_Text) 
		{
			SetGuid(dialogueData_Text.guidID.Value);
			SetTextList(dialogueData_Text.textList);
			SetAudioList(dialogueData_Text.audioClipList);
		}
	}
}