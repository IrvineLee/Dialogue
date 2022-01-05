using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dico.Helper;
using Character.Profile;
using Character.ScriptableObjects;

namespace Character
{
	public class TestCharacterProfile : MonoBehaviour
	{
		public static List<string> sCharacterNameList = new List<string>();
		public static List<Sprite> sCharacterSpriteList = new List<Sprite>();

		[StringListToPopup(typeof(TestCharacterProfile), "sCharacterNameList")]
		[SerializeField] string characterName;

		[SpriteListToPopup(typeof(TestCharacterProfile), "sCharacterSpriteList")]
		[SerializeField] string characterSprite;

		string characterProfilePath = "Character/CharacterProfiles";

		void OnValidate()
		{
			CharacterProfilesSO characterProfilesSO = Resources.Load<CharacterProfilesSO>(characterProfilePath);

			sCharacterNameList?.Clear();
			sCharacterSpriteList?.Clear();

			foreach (CharacterProfile profile in characterProfilesSO.CharacterProfileList)
			{
				sCharacterNameList.Add(profile.CharacterName);

				if (string.Equals(characterName, profile.CharacterName))
				{
					foreach (Sprite sprite in profile.CharacterSpriteList)
					{
						sCharacterSpriteList.Add(sprite);
					}
				}
			}
		}
	}
}