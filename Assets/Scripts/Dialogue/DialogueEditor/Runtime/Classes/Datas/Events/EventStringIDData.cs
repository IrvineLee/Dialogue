using System;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class EventStringIDData
	{
		[SerializeField] string stringEvent;
		[SerializeField] int idNumber;

		public string StringEvent { get => stringEvent; }
		public int IdNumber { get => idNumber; }

		public EventStringIDData() { }

		public EventStringIDData(string stringEvent, int idNumber)
		{
			this.stringEvent = stringEvent;
			this.idNumber = idNumber;
		}

		public void SetStringEvent(string stringEvent) { this.stringEvent = stringEvent; }
		public void SetIDNumber(int idNumber) { this.idNumber = idNumber; }
	}
}