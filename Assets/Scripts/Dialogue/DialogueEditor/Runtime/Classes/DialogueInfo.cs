using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor.Runtime.Enums.Nodes;

namespace DialogueEditor.Runtime.Classes
{
	[Serializable]
	public class DialogueInfo
	{
		[SerializeField] string characterName = "";
		[SerializeField] Sprite characterPotrait;
		[SerializeField] PotraitFaceImageDirection potraitFaceImageDirection;

		[SerializeField] List<LanguageGeneric<string>> textList = new List<LanguageGeneric<string>>();
		[SerializeField] List<LanguageGeneric<AudioClip>> audioClipList = new List<LanguageGeneric<AudioClip>>();
		[SerializeField] List<DialogueNodePort> dialogueNodePortList = new List<DialogueNodePort>();

		public string CharacterName { get => characterName; }
		public Sprite CharacterPotrait { get => characterPotrait; }
		public PotraitFaceImageDirection PotraitFaceImageDirection { get => potraitFaceImageDirection; }

		public List<LanguageGeneric<string>> TextList { get => textList; }
		public List<LanguageGeneric<AudioClip>> AudioClipList { get => audioClipList; }
		public List<DialogueNodePort> DialogueNodePortList { get => dialogueNodePortList; }

		public void SetCharacterName(string characterName) { this.characterName = characterName; }
		public void SetCharacterPotrait(Sprite characterPotrait) { this.characterPotrait = characterPotrait; }
		public void SetPotraitFaceImageDirection(PotraitFaceImageDirection potraitFaceImageDirection) { this.potraitFaceImageDirection = potraitFaceImageDirection; }

		public DialogueInfo() { }

		public DialogueInfo(DialogueInfo dialogueInfo)
		{
			characterName = dialogueInfo.characterName;
			characterPotrait = dialogueInfo.characterPotrait;
			potraitFaceImageDirection = dialogueInfo.potraitFaceImageDirection;

			textList = dialogueInfo.textList;
			audioClipList = dialogueInfo.audioClipList;
			dialogueNodePortList = new List<DialogueNodePort>(dialogueInfo.dialogueNodePortList);
		}

		public void SetTextList(List<LanguageGeneric<string>> textList) { this.textList = textList; }

		public void SetAudioList(List<LanguageGeneric<AudioClip>> audioClipList) { this.audioClipList = audioClipList; }

		public void UpdateLanguage(List<LanguageGeneric<string>> textList, List<LanguageGeneric<AudioClip>> audioClipList)
		{
			SetTextList(textList);
			SetAudioList(audioClipList);
		}
	}
}