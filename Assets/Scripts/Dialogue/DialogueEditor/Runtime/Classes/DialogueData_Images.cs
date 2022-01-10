using System;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes
{
	[Serializable]
	public class DialogueData_Images : DialogueData_BaseContainer
	{
		[SerializeField] SpriteContainer spriteLeft = new SpriteContainer();
		[SerializeField] SpriteContainer spriteRight = new SpriteContainer();

		public SpriteContainer SpriteLeft { get => spriteLeft; }
		public SpriteContainer SpriteRight { get => spriteRight; }

		public void SetSpriteLeft(Sprite sprite) { spriteLeft.SetValue(sprite); }

		public void SetSpriteRight(Sprite sprite) { spriteRight.SetValue(sprite); }

		public void SetSpriteSprites(Sprite spriteLeft, Sprite spriteRight) 
		{
			SetSpriteLeft(spriteLeft);
			SetSpriteLeft(spriteRight);
		}
	}
}