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

		Texture2D picture;

		public void Configure(DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			this.editorWindow = editorWindow;
			this.graphView = graphView;

			picture = new Texture2D(1, 1);
			picture.SetPixel(0, 0, new Color(0, 0, 0, 0));
			picture.Apply();
		}

		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
		{
			List<SearchTreeEntry> searchTreeEntryList = new List<SearchTreeEntry>
			{
				new SearchTreeGroupEntry(new GUIContent("DialogueNode"), 0),
				new SearchTreeGroupEntry(new GUIContent("DialogueNode"), 1),

				AddNodeSearch("Start Node", new StartNode()),
				AddNodeSearch("Dialogue Node", new DialogueNode()),
				AddNodeSearch("Event Node", new EventNode()),
				AddNodeSearch("End Node", new EndNode()),
			};
			return searchTreeEntryList;
		}

		public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
		{
			Vector2 mousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo
				(editorWindow.rootVisualElement.parent, context.screenMousePosition - editorWindow.position.position);

			Vector2 graphMousePosition = graphView.contentViewContainer.WorldToLocal(mousePosition);

			return CheckForNodeType(searchTreeEntry, graphMousePosition);
		}

		SearchTreeEntry AddNodeSearch(string name, BaseNode baseNode)
		{
			SearchTreeEntry searchTreeEntry = new SearchTreeEntry(new GUIContent(name, picture))
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