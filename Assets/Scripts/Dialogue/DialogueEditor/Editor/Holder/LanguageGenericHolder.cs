using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

using DialogueEditor.Runtime.Classes;

namespace DialogueEditor.Runtime
{
	public class LanguageGenericHolder
	{
		[Serializable]
		public class OfText
		{
			[SerializeField] List<LanguageGeneric<string>> inputText;
			[SerializeField] TextField textField;
			[SerializeField] string placeholderText;

			public List<LanguageGeneric<string>> InputText { get => inputText; }
			public TextField TextField { get => textField; }
			public string PlaceholderText { get => placeholderText; }

			public OfText(List<LanguageGeneric<string>> inputText, TextField textField, string placeholderText = "placeholderText")
			{
				this.inputText = inputText;
				this.textField = textField;
				this.placeholderText = placeholderText;
			}
		}

		[Serializable]
		public class OfAudioClip
		{
			[SerializeField] List<LanguageGeneric<AudioClip>> inputAudioClip;
			[SerializeField] ObjectField objectField;

			public List<LanguageGeneric<AudioClip>> InputAudioClip { get => inputAudioClip; }
			public ObjectField ObjectField { get => objectField; }

			public OfAudioClip(List<LanguageGeneric<AudioClip>> inputAudioClip, ObjectField objectField)
			{
				this.inputAudioClip = inputAudioClip;
				this.objectField = objectField;
			}
		}
	}
}