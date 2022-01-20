using System;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes
{
	[Serializable]
	public class DialogueData_BaseContainer
	{
		[SerializeField] IntContainer id = new IntContainer();

		public IntContainer Id { get => id; }

		public DialogueData_BaseContainer() { }

		public DialogueData_BaseContainer(int id)
		{
			this.id = new IntContainer(id);
		}

		public void SetID(int id) { this.id.SetValue(id); }
		public void SetID(IntContainer id) { this.id = id; }
	}
}