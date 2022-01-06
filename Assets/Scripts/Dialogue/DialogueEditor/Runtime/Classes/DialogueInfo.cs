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
		[SerializeField] PotraitFacingDirection potraitFacingDirection;

		[SerializeField] List<LanguageGeneric<string>> textLanguageList = new List<LanguageGeneric<string>>();
		[SerializeField] List<LanguageGeneric<AudioClip>> audioClipLanguageList = new List<LanguageGeneric<AudioClip>>();
		[SerializeField] List<DialogueNodePort> dialogueNodePortList = new List<DialogueNodePort>();

		public string CharacterName { get => characterName; }
		public Sprite CharacterPotrait { get => characterPotrait; }
		public PotraitFacingDirection PotraitFacingDirection { get => potraitFacingDirection; }

		public List<LanguageGeneric<string>> TextLanguageList { get => textLanguageList; }
		public List<LanguageGeneric<AudioClip>> AudioClipList { get => audioClipLanguageList; }
		public List<DialogueNodePort> DialogueNodePortList { get => dialogueNodePortList; }

		public void SetCharacterName(string characterName) { this.characterName = characterName; }
		public void SetCharacterPotrait(Sprite characterPotrait) { this.characterPotrait = characterPotrait; }
		public void SetPotraitFacingDirection(PotraitFacingDirection potraitFacingDirection) { this.potraitFacingDirection = potraitFacingDirection; }

		public DialogueInfo() { }

		public DialogueInfo(DialogueInfo dialogueInfo)
		{
			characterName = dialogueInfo.characterName;
			characterPotrait = dialogueInfo.characterPotrait;
			potraitFacingDirection = dialogueInfo.potraitFacingDirection;

			textLanguageList = dialogueInfo.textLanguageList;
			audioClipLanguageList = dialogueInfo.audioClipLanguageList;
			dialogueNodePortList = new List<DialogueNodePort>(dialogueInfo.dialogueNodePortList);
		}

		public void SetTextList(List<LanguageGeneric<string>> textLanguageList) { this.textLanguageList = textLanguageList; }

		public void SetAudioList(List<LanguageGeneric<AudioClip>> audioClipLanguageList) { this.audioClipLanguageList = audioClipLanguageList; }

		public void UpdateLanguage(List<LanguageGeneric<string>> textLanguageList, List<LanguageGeneric<AudioClip>> audioClipLanguageList)
		{
			SetTextList(textLanguageList);
			SetAudioList(audioClipLanguageList);
		}
	}
}