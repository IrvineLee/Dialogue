using System;
using UnityEngine;

using DialogueEditor.Runtime.Enums.Nodes;

namespace DialogueEditor.Runtime.Classes.Data
{
	[Serializable]
	public class EventData_StringModifier
	{
		[SerializeField] StringContainer stringEventText = new StringContainer();
		[SerializeField] FloatContainer idNumber = new FloatContainer();

		[SerializeField] EnumContainer<StringEventModifierType> stringEventModifierType = new EnumContainer<StringEventModifierType>();

		public StringContainer StringEventText { get => stringEventText; }
		public FloatContainer IDNumber { get => idNumber; }
		public EnumContainer<StringEventModifierType> StringEventModifierType { get => stringEventModifierType; }

		public EventData_StringModifier() { }

		public EventData_StringModifier(string stringEventText, int idNumber, StringEventModifierType stringEventModifierType)
		{
			this.stringEventText = new StringContainer(stringEventText);
			this.idNumber = new FloatContainer(idNumber);
			this.stringEventModifierType = new EnumContainer<StringEventModifierType>(stringEventModifierType);
		}

		public void SetModifier(EventData_StringModifier eventData)
		{
			stringEventText.SetValue(eventData.stringEventText.Value);
			idNumber.SetValue(eventData.idNumber.Value);
			stringEventModifierType.SetValue(eventData.stringEventModifierType.Value);
		}
	}
}