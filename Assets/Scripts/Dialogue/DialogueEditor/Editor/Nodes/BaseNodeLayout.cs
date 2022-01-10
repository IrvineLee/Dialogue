using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using DialogueEditor.Runtime;
using DialogueEditor.Runtime.Classes.Data;
using DialogueEditor.Runtime.ScriptableObjects;
using DialogueEditor.Runtime.Enums.Nodes;

namespace DialogueEditor.Editor.Nodes
{
	public class BaseNodeLayout : BaseNode
	{
		#region New Dialogue Object Field
		/// <summary>
		/// Get Dialogue Object Field. Optional USS naming.
		/// </summary>
		protected ObjectField GetNewObjectField_DialogueEvent(Container_DialogueEventSO inputValue, Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			ObjectField objectField = new ObjectField()
			{
				objectType = typeof(DialogueEventSO),
				allowSceneObjects = false,
				value = inputValue.Value,
			};
			boxContainer?.Add(objectField);

			objectField = GetNewField(objectField, inputValue, null, USS01, USS02);
			objectField.SetValueWithoutNotify(inputValue.Value);

			return objectField;
		}
		#endregion

		#region New Enum Fields

		/// <summary>
		/// Get ChoiceStateType enum.
		/// </summary>
		protected EnumField GetNewEnumField_ChoiceStateType(EnumContainer<ChoiceStateType> inputValue, Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			return GetNewEnumField_Generic(inputValue, default, boxContainer, USS01, USS02);
		}

		/// <summary>
		/// Get EndNodeType enum.
		/// </summary>
		protected EnumField GetNewEnumField_EndNodeType(EnumContainer<EndNodeType> inputValue, Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			return GetNewEnumField_Generic(inputValue, default, boxContainer, USS01, USS02);
		}

		/// <summary>
		/// Get StringEventModifierType enum.
		/// </summary>
		protected EnumField GetNewEnumField_StringEventModifierType(EnumContainer<StringEventModifierType> inputValue, Action valueChangedCallback,
																	Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			return GetNewEnumField_Generic(inputValue, valueChangedCallback, boxContainer, USS01, USS02);
		}

		/// <summary>
		/// Get StringEventConditionType enum.
		/// </summary>
		protected EnumField GetNewEnumField_StringEventConditionType(EnumContainer<StringEventConditionType> inputValue, Action valueChangedCallback,
																	 Box boxContainer = null, string USS01 = "", string USS02 = "")
		{
			return GetNewEnumField_Generic(inputValue, valueChangedCallback, boxContainer, USS01, USS02);
		}
		#endregion

		#region ---- Custom-Made ----
		/// <summary>
		/// Add String Modifier Event to UI element.
		/// </summary>
		/// <param name="stringEventModifierList"> List<EventData_StringModifier> that EventData_StringModifier should be added to. </param>
		/// <param name="stringEvent"> EventData_StringModifier that should be used. </param>
		protected void AddStringModifierEventBuild(List<EventData_StringModifier> stringEventModifierList, EventData_StringModifier stringEvent = null)
		{
			EventData_StringModifier currentStringEventModifier = new EventData_StringModifier();

			if (stringEvent != null)
				currentStringEventModifier.SetModifier(stringEvent);

			stringEventModifierList.Add(currentStringEventModifier);

			Action<Box, Box> visualElementAct = (boxContainer, boxFloatField) =>
			{
				// Text.
				GetNewTextField(currentStringEventModifier.StringEventText, "String Event", boxContainer, "StringEventText");

				// Enum.
				Action act = () => ShowHide_StringEventModifierType(currentStringEventModifier.StringEventModifierType.Value, boxFloatField);
				GetNewEnumField_StringEventModifierType(currentStringEventModifier.StringEventModifierType, act, boxContainer, "StringEventNum");

				// ID number.
				GetNewFloatField(currentStringEventModifier.IDNumber, boxContainer, "StringEventInt");

				// Show and hide the modifier.
				ShowHide_StringEventModifierType(currentStringEventModifier.StringEventModifierType.Value, boxFloatField);
			};
			Action onDeleteAct = () => { stringEventModifierList.Remove(currentStringEventModifier); };

			Box box = AddBaseBoxContainer(visualElementAct, null, onDeleteAct);
			mainContainer.Add(box);
		}

		/// <summary>
		/// Add String Condition Event to UI element.
		/// </summary>
		/// <param name="stringEventModifierList"> List<EventData_StringModifier> that EventData_StringModifier should be added to. </param>
		/// <param name="stringEvent"> EventData_StringModifier that should be used. </param>
		protected void AddStringConditionEventBuild(List<EventData_StringCondition> stringEventConditionList, EventData_StringCondition stringEvent = null)
		{
			EventData_StringCondition currentStringEventCondition = new EventData_StringCondition();

			if (stringEvent != null)
				currentStringEventCondition.SetModifier(stringEvent);

			stringEventConditionList.Add(currentStringEventCondition);

			Action<Box, Box> visualElementAct = (boxContainer, boxFloatField) =>
			{
				// Text.
				GetNewTextField(currentStringEventCondition.StringEventText, "String Event", boxContainer, "StringEventText");

				// Enum.
				Action act = () => ShowHide_StringEventConditionType(currentStringEventCondition.StringEventConditionType.Value, boxFloatField);
				GetNewEnumField_StringEventConditionType(currentStringEventCondition.StringEventConditionType, act, boxContainer, "StringEventNum");

				// ID number.
				GetNewFloatField(currentStringEventCondition.IDNumber, boxContainer, "StringEventInt");

				// Show and hide the modifier.
				ShowHide_StringEventConditionType(currentStringEventCondition.StringEventConditionType.Value, boxFloatField);
			};
			Action onDeleteAct = () => { stringEventConditionList.Remove(currentStringEventCondition); };

			Box box = AddBaseBoxContainer(visualElementAct, null, onDeleteAct);
			mainContainer.Add(box);
		}

		protected void AddScriptableEventBuild(EventNodeData eventNodeData, Container_DialogueEventSO scriptableEventList = null)
		{
			Container_DialogueEventSO currentScriptableEvent = new Container_DialogueEventSO();

			if (scriptableEventList != null)
				currentScriptableEvent.SetModifier(scriptableEventList.Value);

			eventNodeData.Container_DialogueEventSOList.Add(currentScriptableEvent);

			Action<Box> visualElementAct = (boxContainer) =>
			{
				GetNewObjectField_DialogueEvent(currentScriptableEvent, boxContainer, "EventObject");
			};
			Action onDeleteAct = () => { eventNodeData.Container_DialogueEventSOList.Remove(currentScriptableEvent); };
			
			Box box = AddBaseBoxContainer(visualElementAct, null, onDeleteAct, "EventBox");
			mainContainer.Add(box);
		}

		/// <summary>
		/// Generic Base Box Container which contains a box container that houses a delete button.
		/// </summary>
		/// <param name="visualElementAct"> Other visual elements. 1st box parameter is the Box Container. </param>
		/// <param name="onDeleteAct"> Things to happen on delete. </param>
		protected Box AddBaseBoxContainer(Action<Box> visualElementAct = default, Box deleteButtonContainer = null, 
										  Action onDeleteAct = default, string boxContainerName = "BaseBoxContainer")
		{
			// Container of all object.
			Box boxContainer = new Box();
			boxContainer.AddToClassList(boxContainerName);

			visualElementAct?.Invoke(boxContainer);
			GetDeleteButton(deleteButtonContainer == null ? boxContainer : deleteButtonContainer, boxContainer, onDeleteAct);

			RefreshExpandedState();
			return boxContainer;
		}

		/// <summary>
		/// Generic Base Box Container which contains a box container that houses a float container and a delete button.
		/// </summary>
		/// <param name="visualElementAct"> Other visual elements. 1st box parameter is the Box Container. 2nd box parameter is the float container. </param>
		/// <param name="onDeleteAct"> Things to happen on delete. </param>
		protected Box AddBaseBoxContainer(Action<Box, Box> visualElementAct = default, Box deleteButtonContainer = null, Action onDeleteAct = default)
		{
			// Container of all object.
			Box boxContainer = new Box();
			Box boxFloatField = new Box();
			boxContainer.AddToClassList("StringEventBox");
			boxFloatField.AddToClassList("StringEventBoxFloatField");

			visualElementAct?.Invoke(boxContainer, boxFloatField);
			boxContainer.Add(boxFloatField);

			GetDeleteButton(deleteButtonContainer == null ? boxContainer : deleteButtonContainer, boxContainer, onDeleteAct);

			RefreshExpandedState();
			return boxContainer;
		}

		/// <summary>
		/// Delete button.
		/// </summary>
		protected Button GetDeleteButton(Box deleteButtonContainer, Box boxContainer, Action onDeleteAct = default)
		{
			Action deleteClickedAct = () =>
			{
				onDeleteAct?.Invoke();
				OnDeleteBox(deleteButtonContainer);
			};
			return GetNewButton("X", deleteClickedAct, boxContainer, "deleteButton");
		}

		#endregion

		#region ---- Protected ----
		/// <summary>
		/// Show and hide the modifier type.
		/// </summary>
		protected void ShowHide_StringEventModifierType(StringEventModifierType value, Box boxContainer)
		{
			Func<bool> condition = () => { return value == StringEventModifierType.SetTrue || value == StringEventModifierType.SetFalse; };
			ShowHideCondition_BoxContainer(condition, boxContainer);
		}

		/// <summary>
		/// Show and hide the condition type.
		/// </summary>
		protected void ShowHide_StringEventConditionType(StringEventConditionType value, Box boxContainer)
		{
			Func<bool> condition = () => { return value == StringEventConditionType.True || value == StringEventConditionType.False; };
			ShowHideCondition_BoxContainer(condition, boxContainer);
		}

		protected void ShowHideCondition_BoxContainer(Func<bool> condition, Box boxContainer)
		{
			if (condition())
			{
				ShowHide(false, boxContainer);
				return;
			}
			ShowHide(true, boxContainer);
		}

		protected void ShowHide(bool isShow, Box boxContainer)
		{
			string hideUssClass = "Hide";
			if (isShow)
			{
				boxContainer.RemoveFromClassList(hideUssClass);
				return;
			}
			boxContainer.AddToClassList(hideUssClass);
		}
		#endregion

		#region ---- Protected Virtual ----
		protected virtual void OnDeleteBox(Box deleteButtonContainer)
		{
			if (deleteButtonContainer != null) mainContainer.Remove(deleteButtonContainer);
			RefreshExpandedState();
		}
		#endregion
	}
}