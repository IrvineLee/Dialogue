using System;
using UnityEngine;

using DialogueEditor.Runtime.Enums.Nodes;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class EventData_StringCondition
	{
		[SerializeField] StringContainer stringEventText = new StringContainer();
		[SerializeField] FloatContainer idNumber = new FloatContainer();

		[SerializeField] EnumContainer<StringEventConditionType> stringEventConditionType = new EnumContainer<StringEventConditionType>();

		public StringContainer StringEventText { get => stringEventText; }
		public FloatContainer IDNumber { get => idNumber; }
		public EnumContainer<StringEventConditionType> StringEventConditionType { get => stringEventConditionType; }

		public EventData_StringCondition() { }

		public EventData_StringCondition(string stringEventText, int idNumber, StringEventConditionType stringEventConditionType)
		{
			this.stringEventText = new StringContainer(stringEventText);
			this.idNumber = new FloatContainer(idNumber);
			this.stringEventConditionType = new EnumContainer<StringEventConditionType>(stringEventConditionType);
		}

		public void SetModifier(EventData_StringCondition eventData)
		{
			stringEventText.SetValue(eventData.stringEventText.Value);
			idNumber.SetValue(eventData.idNumber.Value);
			stringEventConditionType.SetValue(eventData.stringEventConditionType.Value);
		}
	}
}