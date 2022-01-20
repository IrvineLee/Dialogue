using System.Collections.Generic;
using System.Linq;

using UnityEditor.Experimental.GraphView;
using UnityEditor;

using DialogueEditor.Editor.GraphView;
using DialogueEditor.Editor.Nodes;
using DialogueEditor.Runtime.ScriptableObjects;
using DialogueEditor.Runtime.Classes.Data;
using DialogueEditor.Runtime.Classes;

namespace DialogueEditor.Editor.SaveLoad
{
	public class DialogueSaveAndLoad
	{
		DialogueGraphView graphView;

		List<Edge> edgeList => graphView.edges.ToList();
		List<BaseNode> nodeList => graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();

		public DialogueSaveAndLoad(DialogueGraphView graphView)
		{
			this.graphView = graphView;
		}

		public void Save(DialogueContainerSO dialogueContainerSO)
		{
			SaveEdges(dialogueContainerSO);
			SaveNodes(dialogueContainerSO);

			EditorUtility.SetDirty(dialogueContainerSO);
			AssetDatabase.SaveAssets();
		}

		public void Load(DialogueContainerSO dialogueContainerSO)
		{
			ClearGraph();
			GenerateNodes(dialogueContainerSO);
			ConnectNodes(dialogueContainerSO);
		}

		#region Save

		void SaveEdges(DialogueContainerSO dialogueContainerSO)
		{
			dialogueContainerSO.NodeLinkDataList.Clear();

			List<Edge> connectedEdgeArray = edgeList.Where(edge => edge.input.node != null).ToList();
			foreach (Edge edge in connectedEdgeArray)
			{
				BaseNode outputNode = (BaseNode)edge.output.node;
				BaseNode inputNode = (BaseNode)edge.input.node;

				dialogueContainerSO.NodeLinkDataList.Add(new NodeLinkData(outputNode.NodeGuid, edge.output.portName, inputNode.NodeGuid, edge.input.portName));
			}
		}

		void SaveNodes(DialogueContainerSO dialogueContainerSO)
		{
			dialogueContainerSO.StartNodeDataList.Clear();
			dialogueContainerSO.DialogueNodeDataList.Clear();
			dialogueContainerSO.BranchNodeDataList.Clear();
			dialogueContainerSO.ChoiceNodeDataList.Clear();
			dialogueContainerSO.EventNodeDataList.Clear();
			dialogueContainerSO.EndNodeDataList.Clear();

			foreach (BaseNode node in nodeList)
			{
				switch (node)
				{
					case StartNode startNode:
						dialogueContainerSO.StartNodeDataList.Add(SaveNodeData(startNode));
						break;
					case DialogueNode dialogueNode:
						dialogueContainerSO.DialogueNodeDataList.Add(SaveNodeData(dialogueNode));
						break;
					case BranchNode branchNode:
						dialogueContainerSO.BranchNodeDataList.Add(SaveNodeData(branchNode));
						break;
					case ChoiceNode choiceNode:
						dialogueContainerSO.ChoiceNodeDataList.Add(SaveNodeData(choiceNode));
						break;
					case EventNode eventNode:
						dialogueContainerSO.EventNodeDataList.Add(SaveNodeData(eventNode));
						break;
					case EndNode endNode:
						dialogueContainerSO.EndNodeDataList.Add(SaveNodeData(endNode));
						break;
					default:
						break;
				}
			}
		}

		StartNodeData SaveNodeData(StartNode node)
		{
			StartNodeData startNodeData = new StartNodeData();
			startNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			return startNodeData;
		}

		DialogueNodeData SaveNodeData(DialogueNode node)
		{
			DialogueNodeData dialogueNodeData = new DialogueNodeData(node.DialogueNodeData.DialogueInfo);
			dialogueNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			// Set ID
			for (int i = 0; i < node.DialogueNodeData.DialogueInfo.BaseContainerList.Count; i++)
			{
				node.DialogueNodeData.DialogueInfo.BaseContainerList[i].SetID(i);
			}

			// Port
			foreach (DialogueData_Port port in node.DialogueNodeData.PortList)
			{
				DialogueData_Port portData = new DialogueData_Port();
				portData.SetGuid(port);

				foreach (Edge edge in edgeList)
				{
					if (edge.output.portName == port.PortGuid)
					{
						portData.SetOutputGuid((edge.output.node as BaseNode).NodeGuid);
						portData.SetInputGuid((edge.input.node as BaseNode).NodeGuid);
					}
				}
				dialogueNodeData.PortList.Add(portData);
			}

			return dialogueNodeData;
		}

		BranchNodeData SaveNodeData(BranchNode node)
		{
			//List<Edge> tmpEdges = edgeList.Where(x => x.output.node == node).Cast<Edge>().ToList();

			Edge trueOutput = edgeList.FirstOrDefault(line => line.output.node == node && string.Equals(line.output.portName, "True"));
			Edge falseOutput = edgeList.FirstOrDefault(line => line.output.node == node && string.Equals(line.output.portName, "False"));

			string trueGuidNode = trueOutput != null ? (trueOutput.input.node as BaseNode).NodeGuid : string.Empty;
			string falseGuidNode = falseOutput != null ? (falseOutput.input.node as BaseNode).NodeGuid : string.Empty;

			BranchNodeData branchNodeData = new BranchNodeData(trueGuidNode, falseGuidNode, node.BranchNodeData.EventData_StringConditionList);
			branchNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			return branchNodeData;
		}

		ChoiceNodeData SaveNodeData(ChoiceNode node)
		{
			ChoiceNodeData temp = node.ChoiceNodeData;

			ChoiceNodeData choiceNodeData = new ChoiceNodeData(temp.ChoiceStateType, temp.TextList, temp.AudioClipList, temp.EventData_StringConditionList);
			choiceNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			return choiceNodeData;
		}

		EventNodeData SaveNodeData(EventNode node)
		{
			EventNodeData eventNodeData = new EventNodeData(node.EventNodeData.EventData_StringModifierList, node.EventNodeData.Container_DialogueEventSOList);
			eventNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			return eventNodeData;
		}

		EndNodeData SaveNodeData(EndNode node)
		{
			EndNodeData endNodeData = new EndNodeData(node.EndNodeData.EndNodeType);
			endNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			return endNodeData;
		}

		#endregion

		#region Load

		void ClearGraph()
		{
			foreach (Edge edge in edgeList)
				graphView.RemoveElement(edge);

			foreach (BaseNode node in nodeList)
				graphView.RemoveElement(node);
		}

		void GenerateNodes(DialogueContainerSO dialogueContainerSO)
		{
			foreach (StartNodeData node in dialogueContainerSO.StartNodeDataList)
			{
				StartNode startNode = graphView.CreateStartNode(node.Position);
				startNode.SetNodeGuid(node.NodeGuid);

				graphView.AddElement(startNode);
			}

			foreach (DialogueNodeData node in dialogueContainerSO.DialogueNodeDataList)
			{
				DialogueNode dialogueNode = graphView.CreateDialogueNode(node.Position);
				dialogueNode.LoadDialogueNode(node);

				graphView.AddElement(dialogueNode);
			}

			foreach (BranchNodeData node in dialogueContainerSO.BranchNodeDataList)
			{
				BranchNode branchNode = graphView.CreateBranchNode(node.Position);
				branchNode.LoadBranchNode(node);

				graphView.AddElement(branchNode);
			}

			foreach (ChoiceNodeData node in dialogueContainerSO.ChoiceNodeDataList)
			{
				ChoiceNode choiceNode = graphView.CreateChoiceNode(node.Position);
				choiceNode.LoadChoiceNode(node);

				graphView.AddElement(choiceNode);
			}

			foreach (EventNodeData node in dialogueContainerSO.EventNodeDataList)
			{
				EventNode eventNode = graphView.CreateEventNode(node.Position);
				eventNode.LoadEventNode(node);

				graphView.AddElement(eventNode);
			}

			foreach (EndNodeData node in dialogueContainerSO.EndNodeDataList)
			{
				EndNode endNode = graphView.CreateEndNode(node.Position);
				endNode.LoadEndNode(node);

				graphView.AddElement(endNode);
			}
		}

		void ConnectNodes(DialogueContainerSO dialogueContainerSO)
		{
			// Make connections for all nodes, except for dialogue nodes.
			foreach (BaseNode baseNode in nodeList)
			{
				List<NodeLinkData> connectionList = dialogueContainerSO.NodeLinkDataList.Where(edge => edge.BaseNodeGuid == baseNode.NodeGuid).ToList();

				List<Port> allOutputPort = baseNode.outputContainer.Children().Where(child => child is Port).Cast<Port>().ToList();

				foreach (NodeLinkData nodeLinkData in connectionList)
				{
					string targetNodeGuid = nodeLinkData.TargetNodeGuid;
					BaseNode targetNode = nodeList.First(node => node.NodeGuid == targetNodeGuid);

					if (targetNode == null)
						continue;

					foreach (Port port in allOutputPort)
					{
						if (port.portName == nodeLinkData.BasePortName)
						{
							int index = connectionList.IndexOf(nodeLinkData);
							LinkNodesTogether((Port)targetNode.inputContainer[0], port);
						}
					}
				}
			}
		}

		void LinkNodesTogether(Port inputPort, Port outputPort)
		{
			Edge edge = new Edge()
			{
				input = inputPort,
				output = outputPort,
			};
			edge.input.Connect(edge);
			edge.output.Connect(edge);

			graphView.Add(edge);
		}

		#endregion
	}
}