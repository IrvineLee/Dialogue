using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor.Runtime.ScriptableObjects;
using DialogueEditor.Runtime.Classes.Data;
using DialogueEditor.Runtime.Classes;
using DialogueEditor.Runtime.Enums.Language;
using DialogueEditor.Editor.CSVTools;

public class UpdateLanguageType
{
	public void UpdateLanguage()
	{
		List<DialogueContainerSO> dialogueContainerSOList = CSVHelper.FindAllObjectsFromResources<DialogueContainerSO>();

		foreach (DialogueContainerSO dialogueContainer in dialogueContainerSOList)
		{
			foreach (DialogueNodeData nodeData in dialogueContainer.DialogueNodeDataList)
			{
				List<LanguageGeneric<string>> textList = UpdateLanguageGeneric(nodeData.DialogueInfo.TextLanguageList);
				List<LanguageGeneric<AudioClip>> audioClipList = UpdateLanguageGeneric(nodeData.DialogueInfo.AudioClipList);
				nodeData.UpdateLanguage(textList, audioClipList);

				foreach (DialogueNodePort nodePort in nodeData.DialogueInfo.DialogueNodePortList)
				{
					List<LanguageGeneric<string>> textLanguageList = UpdateLanguageGeneric(nodePort.TextLanguageList);
					nodePort.SetTextLanguageGenericType(textLanguageList);
				}
			}
		}
	}

	List<LanguageGeneric<T>> UpdateLanguageGeneric<T>(List<LanguageGeneric<T>> languageGenericList)
	{
		List<LanguageGeneric<T>> tempList = new List<LanguageGeneric<T>>();

		foreach (LanguageType languageType in Enum.GetValues(typeof(LanguageType)))
		{
			LanguageGeneric<T> languageGeneric = new LanguageGeneric<T>();
			languageGeneric.SetLanguageType(languageType);
			tempList.Add(languageGeneric);
		}

		foreach (LanguageGeneric<T> languageGeneric in languageGenericList)
		{
			LanguageGeneric<T> existLanguage = tempList.Find(language => language.LanguageType == languageGeneric.LanguageType);
			if (existLanguage != null)
			{
				existLanguage.SetLanguageGenericType(languageGeneric.LanguageGenericType);
			}
		}

		return tempList;
	}
}
