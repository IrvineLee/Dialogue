using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor.Editor.GraphView;

namespace DialogueEditor.Editor.Nodes
{
	public class StartNode : BaseNode
	{
		public StartNode() { }

		public StartNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			this.editorWindow = editorWindow;
			this.graphView = graphView;

			title = "Start";
			SetPosition(new Rect(position, defaultNodeSide));
			nodeGuid = Guid.NewGuid().ToString();

			AddOutputPort("Output");

			RefreshExpandedState();
			RefreshPorts();
		}
	}
}