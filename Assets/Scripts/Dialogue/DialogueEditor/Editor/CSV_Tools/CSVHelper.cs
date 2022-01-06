using DialogueEditor.Runtime.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DialogueEditor.Editor.CSVTools
{
	public static class CSVHelper
	{
		/// <summary>
		/// Find all DialogueContainerSO in Resource folder.
		/// </summary>
		public static List<T> FindAllObjectsFromResources<T>()
		{
			List<T> tempList = new List<T>();
			string resourcePath = Application.dataPath + "/Resources";
			string[] directoryArray = Directory.GetDirectories(resourcePath, "*", SearchOption.AllDirectories);

			foreach (string s in directoryArray)
			{
				string directoryPath = s.Substring(resourcePath.Length + 1);
				T[] resultArray = Resources.LoadAll(directoryPath, typeof(T)).Cast<T>().ToArray();

				foreach (T result in resultArray)
				{
					if (!tempList.Contains(result))
						tempList.Add(result);
				}
			}

			return tempList;
		}

		/// <summary>
		/// Find all DialogueContainerSO in Assets folder.
		/// </summary>
		public static List<DialogueContainerSO> FindAllDialogueContainerSO()
		{
			string[] guidArray = AssetDatabase.FindAssets("t:DialogueContainerSO");

			DialogueContainerSO[] dialogueContainerArray = new DialogueContainerSO[guidArray.Length];

			for (int i = 0; i < guidArray.Length; i++)
			{
				string path = AssetDatabase.GUIDToAssetPath(guidArray[i]);								// Use the guid ID to find the Asset path.
				dialogueContainerArray[i] = AssetDatabase.LoadAssetAtPath<DialogueContainerSO>(path);	// Use path to find and load DialogueContainerSO.
			}

			return dialogueContainerArray.ToList();
		}

	}
}