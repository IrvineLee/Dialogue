using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;
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
			SaveNodes(dialogueContainerSO);
			SaveEdges(dialogueContainerSO);

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
			//DialogueNodeData dialogueNodeData = new DialogueNodeData(node.DialogueInfo);
			//dialogueNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			//foreach (DialogueNodePort nodePort in dialogueNodeData.DialogueInfo.DialogueNodePortList)
			//{
			//	nodePort.SetGuid(string.Empty, string.Empty);

			//	foreach (Edge edge in edgeList)
			//	{
			//		if (edge.output.portName == nodePort.PortGuid)
			//		{
			//			string inputGuid = ((BaseNode)edge.input.node).NodeGuid;
			//			string outputGuid = ((BaseNode)edge.output.node).NodeGuid;

			//			nodePort.SetGuid(inputGuid, outputGuid);
			//		}
			//	}
			//}

			//return dialogueNodeData;
			return null;
		}

		BranchNodeData SaveNodeData(BranchNode node)
		{
			Edge trueOutput = edgeList.FirstOrDefault(line => line.output.node == node && string.Equals(line.output.portName, "True"));
			Edge falseOutput = edgeList.FirstOrDefault(line => line.output.node == node && string.Equals(line.output.portName, "False"));

			string trueGuid = trueOutput != null ? (trueOutput.input.node as BaseNode).NodeGuid : string.Empty;
			string falseGuid = falseOutput != null ? (falseOutput.input.node as BaseNode).NodeGuid : string.Empty;

			//BranchNodeData branchNodeData = new BranchNodeData(trueGuid, falseGuid, node.BranchStringIDDataList);
			//branchNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			//return branchNodeData;
			return null;
		}

		EventNodeData SaveNodeData(EventNode node)
		{
			//EventNodeData eventNodeData = new EventNodeData(node.EventStringIDDataList, node.EventScriptableObjectDataList);
			//eventNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			//return eventNodeData;
			return null;
		}

		EndNodeData SaveNodeData(EndNode node)
		{
			//EndNodeData endNodeData = new EndNodeData(node.EndNodeType);
			//endNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			//return endNodeData;
			return null;
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
				dialogueNode.SetNodeGuid(node.NodeGuid);
				dialogueNode.LoadDialogueNode(node);

				graphView.AddElement(dialogueNode);
			}

			foreach (BranchNodeData node in dialogueContainerSO.BranchNodeDataList)
			{
				BranchNode branchNode = graphView.CreateBranchNode(node.Position);
				branchNode.SetNodeGuid(node.NodeGuid);
				//branchNode.LoadBranchNode(node);

				graphView.AddElement(branchNode);
			}

			foreach (EventNodeData node in dialogueContainerSO.EventNodeDataList)
			{
				EventNode eventNode = graphView.CreateEventNode(node.Position);
				eventNode.SetNodeGuid(node.NodeGuid);

				//foreach (EventStringIDData eventStringIDData in node.EventStringIDDataList)
				//{
				//	eventNode.AddStringEvent(eventStringIDData);
				//}

				//foreach (EventScriptableObjectData eventScriptableObjectData in node.EventScriptableObjectDataList)
				//{
				//	eventNode.AddScriptableEvent(eventScriptableObjectData);
				//}

				graphView.AddElement(eventNode);
			}

			foreach (EndNodeData node in dialogueContainerSO.EndNodeDataList)
			{
				EndNode endNode = graphView.CreateEndNode(node.Position);
				endNode.SetNodeGuid(node.NodeGuid);
				//endNode.SetEndNodeType(node.EndNodeType);

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