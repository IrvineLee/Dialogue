using System;
using System.Collections.Generic;
using UnityEngine;

using DialogueDisplay;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class DialogueNodeData : BaseNodeData
	{
		[SerializeField] DialogueInfo dialogueInfo = new DialogueInfo();

		public DialogueInfo DialogueInfo { get => dialogueInfo; }

		public DialogueNodeData(DialogueInfo dialogueInfo)
		{
			this.dialogueInfo = new DialogueInfo(dialogueInfo);
		}

		public string GetTextLanguageGenericTypeRuntime()
		{
			return dialogueInfo.TextList.Find(text => text.LanguageType == LanguageController.sSingleton.LanguageType).LanguageGenericType;
		}

		public AudioClip GetAudioLanguageGenericTypeRuntime()
		{
			return dialogueInfo.AudioClipList.Find(clip => clip.LanguageType == LanguageController.sSingleton.LanguageType).LanguageGenericType;
		}

		public void SetTextLanguageGenericType(List<LanguageGeneric<string>> textList)
		{
			dialogueInfo.SetTextList(textList);
		}

		public void SetAudioLanguageGenericType(List<LanguageGeneric<AudioClip>> audioClipList)
		{
			dialogueInfo.SetAudioList(audioClipList);
		}

		public void UpdateLanguage(List<LanguageGeneric<string>> textList, List<LanguageGeneric<AudioClip>> audioClipList)
		{
			SetTextLanguageGenericType(textList);
			SetAudioLanguageGenericType(audioClipList);
		}
	}
}