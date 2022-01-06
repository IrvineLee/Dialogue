using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueEditor.Editor.CSVTools
{
	public class CustomTool
	{
		[MenuItem("Custom Tools/Dialogue/Save To CSV")]
		public static void SaveToCSV()
		{
			CSVSaver csvSaver = new CSVSaver();
			csvSaver.Save();

			AssetDatabase.Refresh();
			EditorApplication.Beep();
			Debug.Log("<color=green> Save CSV File successfully! </color>");
		}

		[MenuItem("Custom Tools/Dialogue/Load From CSV")]
		public static void LoadFromCSV()
		{
			CSVLoader csvLoder = new CSVLoader();
			csvLoder.Load();

			AssetDatabase.Refresh();
			EditorApplication.Beep();
			Debug.Log("<color=green> Load CSV File successfully! </color>");
		}

		[MenuItem("Custom Tools/Dialogue/Update Dialogue Languages")]
		public static void UpdateDialogueLanguage()
		{
			UpdateLanguageType updateLanguageType = new UpdateLanguageType();
			updateLanguageType.UpdateLanguage();

			EditorApplication.Beep();
			Debug.Log("<color=green> Update languages successfully! </color>");
		}
	}
}