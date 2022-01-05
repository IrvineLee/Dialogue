using System;
using UnityEngine;

namespace Dico.Helper
{
	public class ListToPopupAttribute : PropertyAttribute
	{
		[SerializeField] protected Type myType;
		[SerializeField] protected string propertyName;

		public Type MyType { get => myType; }
		public string PropertyName { get => propertyName; }

		public ListToPopupAttribute() { }

		protected void Initialize(Type myType, string propertyName)
		{
			this.myType = myType;
			this.propertyName = propertyName;
		}
	}

	public class StringListToPopupAttribute : ListToPopupAttribute
	{
		public StringListToPopupAttribute(Type myType, string propertyName) { Initialize(myType, propertyName); }
	}

	public class SpriteListToPopupAttribute : ListToPopupAttribute 
	{
		public SpriteListToPopupAttribute(Type myType, string propertyName) { Initialize(myType, propertyName); }
	}
}