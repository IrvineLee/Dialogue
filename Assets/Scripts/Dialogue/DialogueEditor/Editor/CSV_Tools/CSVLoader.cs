using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using DialogueEditor.Runtime.Classes;
using DialogueEditor.Runtime.Classes.Data;
using DialogueEditor.Runtime.Enums.Language;
using DialogueEditor.Runtime.ScriptableObjects;
using UnityEditor;

namespace DialogueEditor.Editor.CSVTools
{
	public class CSVLoader
	{
		//string csvDirectioryName = "Resources/Dialogue/CSVFiles";
		//string csvFileName = "DialogueCSV_Load.csv";

		public void Load()
		{
			//List<List<string>> resultList = CSVReader.ParseCSV(File.ReadAllText($"{Application.dataPath}/{csvDirectioryName}/{csvFileName}"));
			//List<string> headerList = resultList[0];

			//List<DialogueContainerSO> dialogueContainerList = CSVHelper.FindAllObjectsFromResources<DialogueContainerSO>();

			//foreach (DialogueContainerSO dialogueContainer in dialogueContainerList)
			//{
			//	foreach (DialogueNodeData nodeData in dialogueContainer.DialogueNodeDataList)
			//	{
			//		LoadIntoNode(resultList, headerList, nodeData);
			//		foreach (DialogueNodePort nodePort in nodeData.DialogueInfo.DialogueNodePortList)
			//		{
			//			LoadIntoNodePort(resultList, headerList, nodePort);
			//		}
			//	}
			//	EditorUtility.SetDirty(dialogueContainer);
			//	AssetDatabase.SaveAssets();
			//}
		}

		void LoadIntoNode(List<List<string>> resultList, List<string> headerList, DialogueNodeData nodeData)
		{
			//Action<LanguageType, string> setLanguageGenericAct = (languageType, languageGenericType) =>
			//{
			//	nodeData.DialogueInfo.TextLanguageList.Find(text => text.LanguageType == languageType).SetLanguageGenericType(languageGenericType);
			//};

			//LoadIntoCorrectLanguage(resultList, headerList, nodeData.NodeGuid, setLanguageGenericAct);
		}

		void LoadIntoNodePort(List<List<string>> resultList, List<string> headerList, DialogueNodePort nodePort)
		{
			//Action<LanguageType, string> setLanguageGenericAct = (languageType, languageGenericType) =>
			//{
			//	nodePort.TextLanguageList.Find(text => text.LanguageType == languageType).SetLanguageGenericType(languageGenericType);
			//};

			//LoadIntoCorrectLanguage(resultList, headerList, nodePort.PortGuid, setLanguageGenericAct);
		}

		void LoadIntoCorrectLanguage(List<List<string>> resultList, List<string> headerList, string id, Action<LanguageType, string> setLanguageGenericAct)
		{
			//foreach (List<string> line in resultList)
			//{
			//	if (string.Equals(line[0], id))
			//	{
			//		for (int i = 0; i < line.Count; i++)
			//		{
			//			foreach (LanguageType languageType in Enum.GetValues(typeof(LanguageType)))
			//			{
			//				if (string.Equals(headerList[i], languageType.ToString()))
			//				{
			//					setLanguageGenericAct(languageType, line[i]);
			//				}
			//			}
			//		}
			//	}
			//}
		}
	}
}