using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using DialogueEditor.Runtime.Enums.Language;
using DialogueEditor.Runtime.ScriptableObjects;
using DialogueEditor.Runtime.Classes.Data;
using DialogueEditor.Runtime.Classes;

namespace DialogueEditor.Editor.CSVTools
{
	public class CSVSaver
	{
		string csvDirectioryName = "Resources/Dialogue/CSVFiles";
		string csvFileName = "DialogueCSV_Save.csv";
		string csvSeparator = ",";
		List<string> csvHeaderList;

		string idName = "Guid ID";
		string dialogueName = "Dialogue Name";

		public void Save()
		{
			List<DialogueContainerSO> dialogueContainerList = CSVHelper.FindAllObjectsFromResources<DialogueContainerSO>();

			CreateFile();
			foreach (DialogueContainerSO dialogueContainer in dialogueContainerList)
			{
				foreach (DialogueNodeData nodeData in dialogueContainer.DialogueNodeDataList)
				{
					List<string> stringList = new List<string>();
					stringList.Add(nodeData.NodeGuid);
					stringList.Add(dialogueContainer.name);

					foreach (LanguageType languageType in Enum.GetValues(typeof(LanguageType)))
					{
						string s = nodeData.DialogueInfo.TextLanguageList.Find(language => language.LanguageType == languageType).LanguageGenericType.Replace("\"", "\"\"");
						stringList.Add($"\"{s}\"");
					}

					AppendToFile(stringList);

					foreach (DialogueNodePort nodePort in nodeData.DialogueInfo.DialogueNodePortList)
					{
						stringList = new List<string>();
						stringList.Add(nodePort.PortGuid);
						stringList.Add(dialogueContainer.name);

						foreach (LanguageType languageType in Enum.GetValues(typeof(LanguageType)))
						{
							string s = nodePort.TextLanguageList.Find(language => language.LanguageType == languageType).LanguageGenericType.Replace("\"", "\"\"");
							stringList.Add($"\"{s}\"");
						}

						AppendToFile(stringList);
					}
				}
			}
		}

		void AppendToFile(List<string> stringList)
		{
			using (StreamWriter sw = File.AppendText(GetFilePath()))
			{
				string finalString = GetFinalString(stringList);
				sw.WriteLine(finalString);
			}
		}

		void CreateFile()
		{
			VerifyDirectory();
			MakeHeader();

			using (StreamWriter sw = File.CreateText(GetFilePath()))
			{
				string finalString = GetFinalString(csvHeaderList);
				sw.WriteLine(finalString);
			}
		}

		string GetFinalString(List<string> stringList)
		{
			string finalString = "";
			foreach (string s in stringList)
			{
				if (!string.IsNullOrEmpty(finalString))
				{
					finalString += csvSeparator;
				}
				finalString += s;
			}

			return finalString;
		}

		void VerifyDirectory()
		{
			string directory = GetDirectoryPath();

			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
		}

		void MakeHeader()
		{
			List<string> headerText = new List<string>();
			headerText.Add(idName);
			headerText.Add(dialogueName);

			foreach (LanguageType language in Enum.GetValues(typeof(LanguageType)))
			{
				headerText.Add(language.ToString());
			}

			csvHeaderList = headerText;
		}

		string GetDirectoryPath()
		{
			return $"{Application.dataPath}/{csvDirectioryName}";
		}

		string GetFilePath()
		{
			return $"{GetDirectoryPath()}/{csvFileName}";
		}
	}
}