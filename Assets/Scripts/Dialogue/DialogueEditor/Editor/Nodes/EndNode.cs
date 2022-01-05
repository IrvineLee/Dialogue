using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

using DialogueEditor.Runtime.Enums.Nodes;
using DialogueEditor.Editor.GraphView;

namespace DialogueEditor.Editor.Nodes
{
	public class EndNode : BaseNode
	{
		EndNodeType endNodeType = EndNodeType.End;
		EnumField enumField;

		public EndNodeType EndNodeType { get => endNodeType; }

		public EndNode() { }

		public EndNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			this.editorWindow = editorWindow;
			this.graphView = graphView;

			title = "End";
			SetPosition(new Rect(position, defaultNodeSide));
			nodeGuid = Guid.NewGuid().ToString();

			AddInputPort("Input");

			enumField = new EnumField() { value = endNodeType };
			enumField.Init(endNodeType);
			enumField.RegisterValueChangedCallback((value) => endNodeType = (EndNodeType)value.newValue);
			enumField.SetValueWithoutNotify(endNodeType);

			mainContainer.Add(enumField);
		}

		public void SetEndNodeType(EndNodeType endNodeType) 
		{ 
			this.endNodeType = endNodeType;
			LoadValueIntoField();
		}

		public override void LoadValueIntoField()
		{
			base.LoadValueIntoField();
			enumField.SetValueWithoutNotify(endNodeType);
		}
	}
}