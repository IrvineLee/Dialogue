using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class DialogueNodeData : BaseNodeData
	{
		[SerializeField] List<DialogueData_BaseContainer> baseContainerList = new List<DialogueData_BaseContainer>();
		[SerializeField] List<DialogueData_Name> nameList = new List<DialogueData_Name>();
		[SerializeField] List<DialogueData_Text> textList = new List<DialogueData_Text>();
		[SerializeField] List<DialogueData_Images> imagesList = new List<DialogueData_Images>();
		[SerializeField] List<DialogueData_Port> portList = new List<DialogueData_Port>();

		public List<DialogueData_BaseContainer> BaseContainerList { get => baseContainerList; }
		public List<DialogueData_Name> NameList { get => nameList; }
		public List<DialogueData_Text> TextList { get => textList; }
		public List<DialogueData_Images> ImagesList { get => imagesList; }
		public List<DialogueData_Port> PortList { get => portList; }
	}
}