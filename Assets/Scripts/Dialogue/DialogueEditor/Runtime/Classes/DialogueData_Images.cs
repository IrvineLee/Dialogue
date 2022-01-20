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

		public void SetSpriteLeft(SpriteContainer sprite) { spriteLeft.SetValue(sprite.Value); }

		public void SetSpriteRight(SpriteContainer sprite) { spriteRight.SetValue(sprite.Value); }

		public void SetValues(DialogueData_Images dialogueData_Images)
		{
			SetSpriteLeft(dialogueData_Images.spriteLeft);
			SetSpriteRight(dialogueData_Images.spriteRight);
		}
	}
}