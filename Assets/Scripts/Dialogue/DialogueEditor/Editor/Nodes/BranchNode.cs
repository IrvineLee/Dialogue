using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

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

		public void LoadBranchNode(BranchNodeData node)
		{
			SetNodeGuid(node.NodeGuid);

			foreach (EventData_StringCondition item in node.EventData_StringConditionList)
			{
				AddCondition(item);
			}

			LoadValueIntoField();
			ReloadLanguage();
		}

		void TopButton()
		{
			ToolbarMenu toolbarMenu = new ToolbarMenu();
			toolbarMenu.text = "Add Condition";

			toolbarMenu.menu.AppendAction("String Condition", new Action<DropdownMenuAction>(x => AddCondition()));

			titleContainer.Add(toolbarMenu);
		}

		void AddCondition(EventData_StringCondition stringEvent = null)
		{
			AddStringConditionEventBuild(branchNodeData.EventData_StringConditionList, stringEvent);
		}
	}
}