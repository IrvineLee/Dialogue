using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class DialogueNodeData : BaseNodeData
	{
		[SerializeField] DialogueInfo dialogueInfo = new DialogueInfo();
        [SerializeField] List<DialogueData_Port> portList = new List<DialogueData_Port>();
        public List<DialogueData_Port> PortList { get => portList; }

        public DialogueInfo DialogueInfo { get => dialogueInfo; set => dialogueInfo = value; }

		public DialogueNodeData() { }

		public DialogueNodeData(DialogueInfo dialogueInfo)
		{
			this.dialogueInfo = new DialogueInfo(dialogueInfo);
		}
	}
}