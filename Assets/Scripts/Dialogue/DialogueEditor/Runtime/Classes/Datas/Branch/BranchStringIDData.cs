using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class BranchStringIDData
	{
		[SerializeField] string stringEvent;
		[SerializeField] int idNumber;

		public string StringEvent { get => stringEvent; }
		public int IdNumber { get => idNumber; }

		public BranchStringIDData() { }

		public BranchStringIDData(string stringEvent, int idNumber)
		{
			this.stringEvent = stringEvent;
			this.idNumber = idNumber;
		}

		public void SetStringEvent(string stringEvent) { this.stringEvent = stringEvent; }
		public void SetIDNumber(int idNumber) { this.idNumber = idNumber; }
	}
}