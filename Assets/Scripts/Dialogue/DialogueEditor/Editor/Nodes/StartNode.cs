using UnityEngine;
using UnityEngine.UIElements;

using DialogueEditor.Editor.GraphView;

namespace DialogueEditor.Editor.Nodes
{
	public class StartNode : BaseNodeLayout
	{
		string nodeStyleSheet = "USS/Nodes/StartNodeStyleSheet";

		public StartNode() { }

		public StartNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			StyleSheet styleSheet = Resources.Load<StyleSheet>(nodeStyleSheet);
			Initialize(position, editorWindow, graphView, "Start", styleSheet);

			AddOutputPort("Output");

			RefreshExpandedState();
			RefreshPorts();
		}
	}
}