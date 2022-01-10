using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

using DialogueEditor.Editor.GraphView;
using DialogueEditor.Runtime.Classes.Data;

namespace DialogueEditor.Editor.Nodes
{
	public class EndNode : BaseNode
	{
		[SerializeField] EndNodeData endNodeData = new EndNodeData();

		string nodeStyleSheet = "USS/Nodes/EndNodeStyleSheet";

		public EndNodeData EndNodeData { get => endNodeData; }

		public EndNode() { }

		public EndNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			StyleSheet styleSheet = Resources.Load<StyleSheet>(nodeStyleSheet);
			Initialize(position, editorWindow, graphView, "End", styleSheet);

			AddInputPort("Input");

			EnumField enumField = GetNewEnumField_Generic(endNodeData.EndNodeType);
			mainContainer.Add(enumField);
		}

		public override void LoadValueIntoField()
		{
			if (EndNodeData.EndNodeType.EnumField != null)
				EndNodeData.EndNodeType.EnumField.SetValueWithoutNotify(endNodeData.EndNodeType.Value);
		}
	}
}