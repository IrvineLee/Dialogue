using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

using DialogueEditor.Editor.Nodes;

namespace DialogueEditor.Editor.GraphView
{
	public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
	{
		DialogueEditorWindow editorWindow;
		DialogueGraphView graphView;

		Texture2D iconImage;

		public void Configure(DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			this.editorWindow = editorWindow;
			this.graphView = graphView;

			// Icon image that we don't really use.
			// Use it to create space left of the text.
			iconImage = new Texture2D(1, 1);
			iconImage.SetPixel(0, 0, new Color(0, 0, 0, 0));
			iconImage.Apply();
		}

		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
		{
			List<SearchTreeEntry> searchTreeEntryList = new List<SearchTreeEntry>
			{
				new SearchTreeGroupEntry(new GUIContent("Dialogue Editor"), 0),
				new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),

				AddNodeSearch("Start Node", new StartNode()),
				AddNodeSearch("Dialogue Node", new DialogueNode()),
				AddNodeSearch("Event Node", new EventNode()),
				AddNodeSearch("End Node", new EndNode()),
			};
			return searchTreeEntryList;
		}

		public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
		{
			// Get mouse position on the screen.
			Vector2 mousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo
				(editorWindow.rootVisualElement.parent, context.screenMousePosition - editorWindow.position.position);

			// Use the mouse position to calculate where it is in the graph view.
			Vector2 graphMousePosition = graphView.contentViewContainer.WorldToLocal(mousePosition);

			return CheckForNodeType(searchTreeEntry, graphMousePosition);
		}

		SearchTreeEntry AddNodeSearch(string name, BaseNode baseNode)
		{
			SearchTreeEntry searchTreeEntry = new SearchTreeEntry(new GUIContent(name, iconImage))
			{
				level = 2,
				userData = baseNode,
			};
			return searchTreeEntry;
		}

		bool CheckForNodeType(SearchTreeEntry searchTreeEntry, Vector2 position)
		{
			switch(searchTreeEntry.userData)
			{
				case StartNode node:
					graphView.AddElement(graphView.CreateStartNode(position));
					return true;
				case DialogueNode node:
					graphView.AddElement(graphView.CreateDialogueNode(position));
					return true;
				case EventNode node:
					graphView.AddElement(graphView.CreateEventNode(position));
					return true;
				case EndNode node:
					graphView.AddElement(graphView.CreateEndNode(position));
					return true;
				default:
					break;
			}
			return false;
		}
	}
}