using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

using DialogueEditor.Editor.Nodes;

namespace DialogueEditor.Editor.GraphView
{
	public class DialogueGraphView : UnityEditor.Experimental.GraphView.GraphView
	{
		string graphViewStyleSheet = "GraphViewStyleSheet";			// Name of the graph view style sheet.
		DialogueEditorWindow editorWindow;
		NodeSearchWindow searchWindow;

		public DialogueGraphView(DialogueEditorWindow editorWindow)
		{
			this.editorWindow = editorWindow;

			// Add the ability to zoom in an out graph view.
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

			// Add style sheet to graph view.
			StyleSheet styleSheet = Resources.Load<StyleSheet>(graphViewStyleSheet);
			styleSheets.Add(styleSheet);

			this.AddManipulator(new ContentDragger());		// Ability to drag nodes around.
			this.AddManipulator(new SelectionDragger());	// Ability to drag all selected nodes around.
			this.AddManipulator(new RectangleSelector());	// Ability to drag select a rectangle area.
			this.AddManipulator(new FreehandSelector());	// Ability to select a single node.

			GridBackground grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();

			AddSearchWindow();
		}

		// Override the graph view.
		// This is where we tell the graph view whic nodes can connect to each other.
		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			List<Port> compatiblePorts = new List<Port>();
			Port startPortView = startPort;

			ports.ForEach(port =>
			{
				Port portView = port;

				// 1st: Cannot connect to itself.
				// 2nd: Cannot connect to a port on the same node.
				// 3rd: Input node cannot connect to input node and output node cannot connect to output node.
				if (startPortView != portView && startPortView.node != portView.node && startPortView.direction != port.direction)
					compatiblePorts.Add(port);
			});

			// Return all acceptable ports.
			return compatiblePorts;
		}

		/// <summary>
		/// Reload the current selected language. Used when changing language.
		/// </summary>
		public void ReloadLanguage()
		{
			List<BaseNode> allNodeList = nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();
			foreach (BaseNode node in allNodeList)
			{
				node.ReloadLanguage();
			}
		}

		public StartNode CreateStartNode(Vector2 position)
		{
			return new StartNode(position, editorWindow, this);
		}

		public EndNode CreateEndNode(Vector2 position)
		{
			return new EndNode(position, editorWindow, this);
		}

		public EventNode CreateEventNode(Vector2 position)
		{
			return new EventNode(position, editorWindow, this);
		}

		public DialogueNode CreateDialogueNode(Vector2 position)
		{
			return new DialogueNode(position, editorWindow, this);
		}

		void AddSearchWindow()
		{
			searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
			searchWindow.Configure(editorWindow, this);
			nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
		}
	}
}
