using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

using DialogueEditor.Editor.GraphView;
using DialogueEditor.Runtime.ScriptableObjects;

namespace DialogueEditor.Editor.Nodes
{
	public class EventNode : BaseNode
	{
		DialogueEventSO dialogueEvent;
		ObjectField objectField;

		public DialogueEventSO DialogueEvent { get => dialogueEvent; }

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

			objectField = new ObjectField()
			{
				objectType = typeof(DialogueEventSO),
				allowSceneObjects = false,
				value = dialogueEvent
			};

			objectField.RegisterValueChangedCallback(value => dialogueEvent = objectField.value as DialogueEventSO);
			objectField.SetValueWithoutNotify(dialogueEvent);

			mainContainer.Add(objectField);
		}

		public void SetDialogueEventSO(DialogueEventSO dialogueEvent) 
		{ 
			this.dialogueEvent = dialogueEvent;
			LoadValueIntoField();
		}

		public override void LoadValueIntoField()
		{
			base.LoadValueIntoField();
			objectField.SetValueWithoutNotify(dialogueEvent);
		}
	}
}