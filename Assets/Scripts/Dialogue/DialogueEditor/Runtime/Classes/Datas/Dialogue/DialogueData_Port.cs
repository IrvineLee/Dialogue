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
			inputGuid = string.Empty;
			outputGuid = string.Empty;
		}

		public string PortGuid { get => portGuid; }
		public string InputGuid { get => inputGuid; }
		public string OutputGuid { get => outputGuid; }

		public void SetPortGuid(string portGuid) { this.portGuid = portGuid; }
		public void SetInputGuid(string inputGuid) { this.inputGuid = inputGuid; }
		public void SetOutputGuid(string outputGuid) { this.outputGuid = outputGuid; }

		public void SetGuid(DialogueData_Port dialogueData_Port)
		{
			SetValues(dialogueData_Port.PortGuid, dialogueData_Port.InputGuid, dialogueData_Port.OutputGuid);
		}

		void SetValues(string portGuid, string inputGuid, string outputGuid)
		{
			this.portGuid = portGuid;
			this.inputGuid = inputGuid;
			this.outputGuid = outputGuid;
		}
	}
}