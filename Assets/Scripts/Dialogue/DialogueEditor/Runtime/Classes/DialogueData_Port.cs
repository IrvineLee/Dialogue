using System;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes
{
	[Serializable]
	public class DialogueData_Port
	{
		[SerializeField] string portGuid;
		[SerializeField] string inputGuid;
		[SerializeField] string outputGuid;

		public DialogueData_Port()
		{
			portGuid = Guid.NewGuid().ToString();
		}

		public string PortGuid { get => portGuid; }
		public string InputGuid { get => inputGuid; }
		public string OutputGuid { get => outputGuid; }

		public void SetPortGuid(string portGuid) { this.portGuid = portGuid; }
		public void SetInputGuid(string inputGuid) { this.inputGuid = inputGuid; }
		public void SetOutputGuid(string outputGuid) { this.portGuid = outputGuid; }

		public void SetGuid(DialogueNodePort dialogueNodePort)
		{
			portGuid = dialogueNodePort.PortGuid;
			inputGuid = dialogueNodePort.InputGuid;
			outputGuid = dialogueNodePort.OutputGuid;
		}
	}
}