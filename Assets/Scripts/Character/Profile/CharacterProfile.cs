using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Profile
{
	[Serializable]
	public class CharacterProfile
	{
		[SerializeField] string characterName = "";
		[SerializeField] List<Sprite> characterSpriteList = new List<Sprite>();

		public string CharacterName { get => characterName; }
		public List<Sprite> CharacterSpriteList { get => characterSpriteList; }
	}
}