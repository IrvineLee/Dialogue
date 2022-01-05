using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor.Runtime.ScriptableObjects
{
	[Serializable]
	public class DialogueEventSO : ScriptableObject
	{
		public virtual void RunEvent()
		{
			Debug.Log("Event was called");
		}
	}
}