using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

using DialogueEditor.Editor.GraphView;
using UnityEngine.UIElements;

namespace DialogueEditor.Editor.Nodes
{
	public class BaseNode : Node
	{
		protected string nodeGuid;
		protected DialogueGraphView graphView;
		protected DialogueEditorWindow editorWindow;
		protected Vector2 defaultNodeSide = new Vector2(200, 250);

		public string NodeGuid { get => nodeGuid; }

		public BaseNode()
		{
			StyleSheet styleSheet = Resources.Load<StyleSheet>("NodeStyleSheet");
			styleSheets.Add(styleSheet);
		}

		public void SetNodeGuid(string nodeGuid) { this.nodeGuid = nodeGuid; }

		public void AddOutputPort(string name, Port.Capacity capacity = Port.Capacity.Single)
		{
			Port outputPort = GetPortInstance(Direction.Output, capacity);
			outputPort.portName = name;
			outputContainer.Add(outputPort);
		}

		public void AddInputPort(string name, Port.Capacity capacity = Port.Capacity.Multi)
		{
			Port inputPort = GetPortInstance(Direction.Input, capacity);
			inputPort.portName = name;
			inputContainer.Add(inputPort);
		}

		public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
		{
			return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
		}

		public virtual void LoadValueIntoField() { }
	}
}