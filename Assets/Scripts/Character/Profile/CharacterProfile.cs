using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Profile
{
	[Serializable]
	public class CharacterProfile
	{
		[SerializeField] Color editorColor = Color.white;
		[SerializeField] string characterName = "";
		[SerializeField] List<Sprite> characterSpriteList = new List<Sprite>();

		public Color EditorColor { get => editorColor; }
		public string CharacterName { get => characterName; }
		public List<Sprite> CharacterSpriteList { get => characterSpriteList; }
	}
}