using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor.Runtime.ScriptableObjects;

namespace Events
{
	[CreateAssetMenu(fileName = "RandomColorEvent", menuName = "Dialogue/New Random Color Event")]
	public class Event_RandomColor : DialogueEventSO
	{
		public override void RunEvent()
		{
			base.RunEvent();
			GameEvent.sSingleton.CallRandomColorEvent();
		}
	}
}