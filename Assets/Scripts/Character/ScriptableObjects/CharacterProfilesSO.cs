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

		/// <summary>
		/// Returns the list of character names and the index of said name.
		/// </summary>
		public List<(string, Color)> GetCharacterNameList(string characterName, out int index)
		{
			index = 0;
			List<(string, Color)> characterNameList = new List<(string, Color)>();

			foreach (CharacterProfile profile in characterProfileList)
			{
				characterNameList.Add((profile.CharacterName, profile.EditorColor));

				if (string.Equals(profile.CharacterName, characterName))
					index = characterNameList.Count - 1;
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

		public List<Sprite> GetCharacterSpriteList(string characterName)
		{
			List<Sprite> characterSpriteList = new List<Sprite>();

			CharacterProfile characterProfile = characterProfileList.Find(profile => profile.CharacterName == characterName);
			characterSpriteList = characterProfile?.CharacterSpriteList;

			if (string.Equals(characterName, "") && characterProfileList.Count > 0)
				characterSpriteList = characterProfileList[0].CharacterSpriteList;

			return characterSpriteList;
		}

		public Sprite GetCharacterSprite(string characterName = "", string spriteName = "")
		{
			List<Sprite> characterSpriteList = GetCharacterSpriteList(characterName);
			foreach (Sprite sprite in characterSpriteList)
			{
				string dataSpriteName = ((UnityEngine.Object)(object)sprite).name;
				if (string.Equals(dataSpriteName, spriteName))
					return sprite;
			}
			return null;
		}
	}
}