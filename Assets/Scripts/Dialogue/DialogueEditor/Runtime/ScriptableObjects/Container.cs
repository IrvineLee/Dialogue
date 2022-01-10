using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

namespace DialogueEditor.Runtime
{
	public class Container<T>
	{
		[SerializeField] protected T value;

		public T Value { get => value; }

		public virtual void SetValue(T value) { this.value = value; }
	}

	[Serializable]
	public class StringContainer : Container<string>
	{
		public StringContainer() { }
		public StringContainer(string value) { SetValue(value); }
	}

	[Serializable]
	public class IntContainer : Container<int>
	{
		public IntContainer() { }
		public IntContainer(int value) { SetValue(value); }
	}

	[Serializable]
	public class FloatContainer : Container<float>
	{
		public FloatContainer() { }
		public FloatContainer(float value) { SetValue(value); }
	}

	[Serializable]
	public class SpriteContainer : Container<Sprite>
	{
		public SpriteContainer() { }
		public SpriteContainer(Sprite value) { SetValue(value); }
	}

	public class EnumContainer<T> : Container<T> where T : Enum
	{
#if UNITY_EDITOR
		[SerializeField] EnumField enumField;
#endif

#if UNITY_EDITOR
		public EnumField EnumField { get => enumField; }
#endif

		public EnumContainer() { }

		public EnumContainer(T value) { SetValue(value); }

		public void SetEnumField(EnumField enumField) { this.enumField = enumField; }
	}
}