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
		string stypeSheetName = "GraphViewStyleSheet";
		DialogueEditorWindow editorWindow;
		NodeSearchWindow searchWindow;

		public DialogueGraphView(DialogueEditorWindow editorWindow)
		{
			this.editorWindow = editorWindow;

			StyleSheet styleSheet = Resources.Load<StyleSheet>(stypeSheetName);
			styleSheets.Add(styleSheet);

			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
			this.AddManipulator(new FreehandSelector());

			GridBackground grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();

			AddSearchWindow();
		}

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			List<Port> compatiblePorts = new List<Port>();
			Port startPortView = startPort;

			ports.ForEach(port =>
			{
				Port portView = port;

				if (startPortView != portView && startPortView.node != portView.node && startPortView.direction != port.direction)
					compatiblePorts.Add(port);
			});

			return compatiblePorts;
		}

		public void LanguageReload()
		{
			List<DialogueNode> dialogueNodeList = nodes.ToList().Where(node => node is DialogueNode).Cast<DialogueNode>().ToList();
			foreach (DialogueNode dialogueNode in dialogueNodeList)
			{
				dialogueNode.ReloadLanguage();
			}
		}

		public StartNode CreateStartNode(Vector2 position)
		{
			StartNode startNode = new StartNode(position, editorWindow, this);
			return startNode;
		}

		public EndNode CreateEndNode(Vector2 position)
		{
			EndNode endNode = new EndNode(position, editorWindow, this);
			return endNode;
		}

		public EventNode CreateEventNode(Vector2 position)
		{
			EventNode eventNode = new EventNode(position, editorWindow, this);
			return eventNode;
		}

		public DialogueNode CreateDialogueNode(Vector2 position)
		{
			DialogueNode dialogueNode = new DialogueNode(position, editorWindow, this);
			return dialogueNode;
		}

		void AddSearchWindow()
		{
			searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
			searchWindow.Configure(editorWindow, this);
			nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
		}
	}
}
