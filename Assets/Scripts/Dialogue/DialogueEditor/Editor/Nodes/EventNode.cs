using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

using DialogueEditor.Editor.GraphView;
using DialogueEditor.Runtime.Classes.Data;

namespace DialogueEditor.Editor.Nodes
{
	public class EventNode : BaseNodeLayout
	{
		[SerializeField] EventNodeData eventNodeData = new EventNodeData();

		string nodeStyleSheet = "USS/Nodes/EventNodeStyleSheet";

		public EventNodeData EventNodeData { get => eventNodeData; }

		public EventNode() { }

		public EventNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			StyleSheet styleSheet = Resources.Load<StyleSheet>(nodeStyleSheet);
			Initialize(position, editorWindow, graphView, "Event", styleSheet);

			AddInputPort("Input");
			AddOutputPort("Output");

			TopButton();
		}

		public void LoadEventNode(EventNodeData node)
		{
			SetNodeGuid(node.NodeGuid);

			foreach (Container_DialogueEventSO item in node.Container_DialogueEventSOList)
			{
				AddScriptableEvent(item);
			}
			foreach (EventData_StringModifier item in node.EventData_StringModifierList)
			{
				AddStringConditionEventBuild(item);
			}

			LoadValueIntoField();
		}

		void TopButton()
		{
			ToolbarMenu menu = new ToolbarMenu();
			menu.text = "Add Event";

			menu.menu.AppendAction("String Event Modifier", new Action<DropdownMenuAction>(x => AddStringConditionEventBuild()));
			menu.menu.AppendAction("Scriptable Object", new Action<DropdownMenuAction>(x => AddScriptableEvent()));

			titleContainer.Add(menu);
		}

		void AddStringConditionEventBuild(EventData_StringModifier stringModifier = null)
		{
			AddStringModifierEventBuild(eventNodeData.EventData_StringModifierList, stringModifier);
		}

		void AddScriptableEvent(Container_DialogueEventSO dialogueEvent = null)
		{
			AddScriptableEventBuild(eventNodeData, dialogueEvent);
		}
	}
}