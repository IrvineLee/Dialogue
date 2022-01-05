using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

using DialogueDisplay;

namespace DialogueEditor.Runtime.Classes
{
	[Serializable]
	public class DialogueNodePort
	{
		[SerializeField] string portGuid, inputGuid, outputGuid;
		[SerializeField] Port myPort;
		[SerializeField] TextField textField;
		[SerializeField] List<LanguageGeneric<string>> textLanguageList = new List<LanguageGeneric<string>>();

		public string PortGuid { get => portGuid; }
		public string InputGuid { get => inputGuid; }
		public string OutputGuid { get => outputGuid; }
		public Port MyPort { get => myPort; }
		public TextField TextField { get => textField; }
		public List<LanguageGeneric<string>> TextLanguageList { get => textLanguageList; }

		public DialogueNodePort() { }

		public DialogueNodePort(string portGuid, string inputGuid, string outputGuid, Port myPort, TextField textField, List<LanguageGeneric<string>> textLanguageList)
		{
			this.portGuid = portGuid;
			this.inputGuid = inputGuid;
			this.outputGuid = outputGuid;
			this.myPort = myPort;
			this.textField = textField;
			this.textLanguageList = textLanguageList;
		}

		public void SetGuid(string inputGuid, string outputGuid)
		{
			this.inputGuid = inputGuid;
			this.outputGuid = outputGuid;
		}

		public string GetTextLanguageGenericTypeRuntime()
		{
			return textLanguageList.Find(text => text.LanguageType == LanguageController.sSingleton.LanguageType).LanguageGenericType;
		}

		public void SetTextLanguageGenericType(List<LanguageGeneric<string>> textLanguageList)
		{
			this.textLanguageList = textLanguageList;
		}
	}
}