using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DialogueEditor.Editor.CSVTools
{
	public static class CSVHelper
	{
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
	}
}