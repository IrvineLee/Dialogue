using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor.Runtime.Enums.Language;

namespace DialogueEditor.Runtime.Classes
{
	[Serializable]
	public class LanguageGeneric<T>
	{
		[SerializeField] LanguageType languageType;
		[SerializeField] T languageGenericType;

		public LanguageType LanguageType { get => languageType; }
		public T LanguageGenericType { get => languageGenericType; }

		public LanguageGeneric() { }
		public LanguageGeneric(LanguageType languageType, T languageGenericType)
		{
			this.languageType = languageType;
			this.languageGenericType = languageGenericType;
		}

		public void SetLanguageType(LanguageType languageType)
		{
			this.languageType = languageType;
		}

		public void SetLanguageGenericType(T languageGenericType)
		{
			this.languageGenericType = languageGenericType;
		}
	}
}