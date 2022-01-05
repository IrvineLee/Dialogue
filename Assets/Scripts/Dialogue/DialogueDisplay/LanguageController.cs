using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor.Runtime.Enums.Language;

namespace DialogueDisplay
{
	public class LanguageController : MonoBehaviour
	{
		[SerializeField] LanguageType languageType = LanguageType.English;

		public static LanguageController sSingleton { get; private set; }
		public LanguageType LanguageType { get => languageType; }

		void Awake()
		{
			if (sSingleton != null && sSingleton != this) Destroy(this.gameObject);
			else sSingleton = this;

			DontDestroyOnLoad(gameObject);
		}
	}
}