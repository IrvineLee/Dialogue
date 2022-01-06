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
			return dialogueInfo.TextLanguageList.Find(text => text.LanguageType == LanguageController.sSingleton.LanguageType).LanguageGenericType;
		}

		public AudioClip GetAudioLanguageGenericTypeRuntime()
		{
			return dialogueInfo.AudioClipList.Find(clip => clip.LanguageType == LanguageController.sSingleton.LanguageType).LanguageGenericType;
		}

		public void SetTextLanguageGenericType(List<LanguageGeneric<string>> textLanguageList)
		{
			dialogueInfo.SetTextList(textLanguageList);
		}

		public void SetAudioLanguageGenericType(List<LanguageGeneric<AudioClip>> audioClipLanguageList)
		{
			dialogueInfo.SetAudioList(audioClipLanguageList);
		}

		public void UpdateLanguage(List<LanguageGeneric<string>> textLanguageList, List<LanguageGeneric<AudioClip>> audioClipLanguageList)
		{
			dialogueInfo.UpdateLanguage(textLanguageList, audioClipLanguageList);
		}
	}
}