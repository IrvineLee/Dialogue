using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

using DialogueEditor.Editor.GraphView;
using DialogueEditor.Runtime.ScriptableObjects;
using DialogueEditor.Runtime.Classes.Data;

namespace DialogueEditor.Editor.Nodes
{
	public class EventNode : BaseNode
	{
		[SerializeField] List<EventStringIDData> eventStringIDDataList = new List<EventStringIDData>();
		[SerializeField] List<EventScriptableObjectData> eventScriptableObjectDataList = new List<EventScriptableObjectData>();

		public List<EventStringIDData> EventStringIDDataList { get => eventStringIDDataList; }
		public List<EventScriptableObjectData> EventScriptableObjectDataList { get => eventScriptableObjectDataList; }

		public EventNode() { }

		public EventNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			this.editorWindow = editorWindow;
			this.graphView = graphView;

			title = "Event";
			SetPosition(new Rect(position, defaultNodeSide));
			nodeGuid = Guid.NewGuid().ToString();

			AddInputPort("Input");
			AddOutputPort("Output");

			TopButton();
		}

		public override void LoadValueIntoField()
		{
			base.LoadValueIntoField();
		}

		public void AddStringEvent(EventStringIDData eventStringIDData = null)
		{
			EventStringIDData currentEventStringIDData = new EventStringIDData();

			// If the data is not null, load in the values.
			if (eventStringIDData != null)
				currentEventStringIDData = new EventStringIDData(eventStringIDData.StringEvent, eventStringIDData.IdNumber);

			eventStringIDDataList.Add(currentEventStringIDData);

			// Create a list of visual elements to be added to base box container.
			List<Func<VisualElement>> visualElementFuncList = new List<Func<VisualElement>>();

			Func<VisualElement> visualElementTextFunc = () =>
			{
				// Text Field.
				TextField textField = new TextField();
				textField.AddToClassList("EventText");
				textField.RegisterValueChangedCallback(value =>
				{
					currentEventStringIDData.SetStringEvent(value.newValue);
				});
				textField.SetValueWithoutNotify(currentEventStringIDData.StringEvent);

				return textField;
			};

			Func<VisualElement> visualElementIntFunc = () =>
			{
				// ID Number.
				IntegerField integerField = new IntegerField();
				integerField.AddToClassList("EventInt");
				integerField.RegisterValueChangedCallback(value =>
				{
					currentEventStringIDData.SetIDNumber(value.newValue);
				});
				integerField.SetValueWithoutNotify(currentEventStringIDData.IdNumber);

				return integerField;
			};

			visualElementFuncList.Add(visualElementTextFunc);
			visualElementFuncList.Add(visualElementIntFunc);

			Action onDeleteButtonClicked = () => eventStringIDDataList.Remove(currentEventStringIDData);
			AddBaseBoxContainer(visualElementFuncList, onDeleteButtonClicked);
		}

		public void AddScriptableEvent(EventScriptableObjectData eventScriptableObjectData = null)
		{
			EventScriptableObjectData currentEventScriptableObjectData = new EventScriptableObjectData();

			// If the data is not null, load in the values.
			if (eventScriptableObjectData != null)
				currentEventScriptableObjectData = new EventScriptableObjectData(eventScriptableObjectData.DialogueEventSO);

			eventScriptableObjectDataList.Add(currentEventScriptableObjectData);

			Func<VisualElement> visualElementFunc = () => 
			{
				// Scriptable Object Event.
				ObjectField objectField = new ObjectField()
				{
					objectType = typeof(DialogueEventSO),
					allowSceneObjects = false,
					value = null,
				};

				objectField.RegisterValueChangedCallback(value =>
				{
					currentEventScriptableObjectData.SetDialogueEventSO(value.newValue as DialogueEventSO);
				});
				objectField.SetValueWithoutNotify(currentEventScriptableObjectData.DialogueEventSO);
				objectField.AddToClassList("EventObject");

				return objectField;
			};

			Action onDeleteButtonClicked = () => eventScriptableObjectDataList.Remove(currentEventScriptableObjectData);
			AddBaseBoxContainer(new List<Func<VisualElement>>() { visualElementFunc }, onDeleteButtonClicked);
		}

		void TopButton()
		{
			ToolbarMenu toolbarMenu = new ToolbarMenu();
			toolbarMenu.text = "Add Event";

			toolbarMenu.menu.AppendAction("String ID", new Action<DropdownMenuAction>(x => AddStringEvent()));
			toolbarMenu.menu.AppendAction("Scriptable Object", new Action<DropdownMenuAction>(x => AddScriptableEvent()));

			titleContainer.Add(toolbarMenu);
		}

		void AddBaseBoxContainer(List<Func<VisualElement>> visualElementFuncList, Action onDeleteButtonClickedAct)
		{
			// Container of all object.
			Box boxContainer = new Box();
			boxContainer.AddToClassList("EventBox");

			foreach (Func<VisualElement> visualElementFunc in visualElementFuncList)
			{
				VisualElement visualElement = visualElementFunc();
				boxContainer.Add(visualElement);
			}
			
			// Delete Button.
			Button deleteButton = new Button() { text = "X" };
			deleteButton.clicked += () =>
			{
				DeleteBox(boxContainer);
				onDeleteButtonClickedAct();
			};
			deleteButton.AddToClassList("DeleteButton");
			boxContainer.Add(deleteButton);

			mainContainer.Add(boxContainer);
			RefreshExpandedState();
		}

		void DeleteBox(Box boxContainer)
		{
			mainContainer.Remove(boxContainer);
			RefreshExpandedState();
		}
	}
}