using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

using Character.Profile;

namespace Character.ScriptableObjects
{
	[CreateAssetMenu(fileName = "CharacterProfiles", menuName = "Character/New Character Profiles")]
	[Serializable]
	public class CharacterProfilesSO : ScriptableObject
	{
		[SerializeField] List<CharacterProfile> characterProfileList = new List<CharacterProfile>();

		public List<CharacterProfile> CharacterProfileList { get => characterProfileList; }

		public List<string> GetCharacterNameList()
		{
			List<string> characterNameList = new List<string>();
			foreach (CharacterProfile profile in characterProfileList)
			{
				characterNameList.Add(profile.CharacterName);
			}
			return characterNameList;
		}

		public List<string> GetCharacterSpriteNameList(string characterName = "")
		{
			List<Sprite> characterSpriteList = GetCharacterSpriteList(characterName);
			List<string> characterSpriteNameList = new List<string>();

			if (characterSpriteList.Count > 0)
				characterSpriteNameList = new List<string>(characterSpriteList.Select(sprite => ((UnityEngine.Object)(object)sprite).name).ToList());

			return characterSpriteNameList;
		}

		public List<Sprite> GetCharacterSpriteList(string characterName = "")
		{
			List<Sprite> characterSpriteList = new List<Sprite>();

			CharacterProfile characterProfile = characterProfileList.Find(profile => profile.CharacterName == characterName);
			characterSpriteList = characterProfile?.CharacterSpriteList;

			if (characterName == "" && characterProfileList.Count > 0)
				characterSpriteList = characterProfileList[0].CharacterSpriteList;

			return characterSpriteList;
		}

		public Sprite GetCharacterSprite(string characterName = "", string spriteName = "")
		{
			List<Sprite> characterSpriteList = GetCharacterSpriteList(characterName);
			foreach (Sprite sprite in characterSpriteList)
			{
				string dataSpriteName = ((UnityEngine.Object)(object)sprite).name;
				if (dataSpriteName == spriteName)
					return sprite;
			}
			return null;
		}
	}
}