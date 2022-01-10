using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

using DialogueEditor.Runtime.Classes.Data;
using DialogueEditor.Editor.GraphView;

namespace DialogueEditor.Editor.Nodes
{
	public class BranchNode : BaseNodeLayout
	{
		[SerializeField] BranchNodeData branchNodeData = new BranchNodeData();

		string nodeStyleSheet = "USS/Nodes/BranchNodeStyleSheet";

		public BranchNodeData BranchNodeData { get => branchNodeData; }

		public BranchNode() { }

		public BranchNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
		{
			StyleSheet styleSheet = Resources.Load<StyleSheet>(nodeStyleSheet);
			Initialize(position, editorWindow, graphView, "Branch", styleSheet);

			AddInputPort("Input");
			AddOutputPort("True");
			AddOutputPort("False");

			TopButton();
		}

		void TopButton()
		{
			ToolbarMenu toolbarMenu = new ToolbarMenu();
			toolbarMenu.text = "Add Condition";

			toolbarMenu.menu.AppendAction("String Condition", new Action<DropdownMenuAction>(x => AddCondition()));

			titleContainer.Add(toolbarMenu);
		}

		public void AddCondition(EventData_StringCondition stringEvent = null)
		{
			AddStringConditionEventBuild(branchNodeData.EventData_StringConditionList, stringEvent);
		}
	}
}