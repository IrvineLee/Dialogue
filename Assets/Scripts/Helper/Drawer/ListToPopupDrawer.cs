using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Dico.Helper
{
	[CustomPropertyDrawer(typeof(SpriteListToPopupAttribute))]
	public class SpriteWrapper : ListToPopupDrawer<Sprite> { }

	[CustomPropertyDrawer(typeof(StringListToPopupAttribute))]
	public class StringWrapper : ListToPopupDrawer<string> { }

	public class ListToPopupDrawer<T> : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			ListToPopupAttribute atb = attribute as ListToPopupAttribute;

			List<T> itemList = new List<T>();
			List<string> stringList = new List<string>();

			if (atb.MyType.GetField(atb.PropertyName) != null)
			{
				itemList = atb.MyType.GetField(atb.PropertyName).GetValue(atb.MyType) as List<T>;

				AddItemListToStringList(itemList, stringList);
			}

			if (stringList != null && stringList.Count != 0)
			{
				int selectedIndex = Mathf.Max(stringList.IndexOf(property.stringValue), 0);

				string propertyName = StringHelper.FirstLetterToUpper(property.name);
				propertyName = StringHelper.AddSymbolInFrontOfUpper(' ', propertyName);
					
				selectedIndex = EditorGUI.Popup(position, propertyName, selectedIndex, stringList.ToArray());
				property.stringValue = stringList[selectedIndex];
			}
			else
			{
				EditorGUI.PropertyField(position, property, label);
			}
		}

		void AddItemListToStringList(List<T> itemList, List<string> stringList)
		{
			if (itemList == null) return;

			foreach (T item in itemList)
			{
				// Clicking the plus icon in list inspector creates an empty element, not fully null.
				if (string.Equals(item.ToString(), "null"))
					continue;

				if (item != null)
				{
					string s = "";

					if (typeof(T) == typeof(Sprite))
						s = ((UnityEngine.Object)(object)item).name.ToString();
					else
						s = item.ToString();

					stringList.Add(s);
				}
			}
		}
	}
}
#endif