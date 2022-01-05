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

				dialogueContainerSO.NodeLinkDataList.Add(new NodeLinkData(outputNode.NodeGuid, inputNode.NodeGuid));
			}
		}

		void SaveNodes(DialogueContainerSO dialogueContainerSO)
		{
			dialogueContainerSO.StartNodeDataList.Clear();
			dialogueContainerSO.DialogueNodeDataList.Clear();
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

		DialogueNodeData SaveNodeData(DialogueNode node)
		{
			DialogueNodeData dialogueNodeData = new DialogueNodeData(node.DialogueInfo);
			dialogueNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			foreach (DialogueNodePort nodePort in dialogueNodeData.DialogueInfo.DialogueNodePortList)
			{
				nodePort.SetGuid(string.Empty, string.Empty);

				foreach (Edge edge in edgeList)
				{
					if (edge.output == nodePort.MyPort)
					{
						string inputGuid = ((BaseNode)edge.input.node).NodeGuid;
						string outputGuid = ((BaseNode)edge.output.node).NodeGuid;

						nodePort.SetGuid(inputGuid, outputGuid);
					}
				}
			}

			return dialogueNodeData;
		}

		StartNodeData SaveNodeData(StartNode node)
		{
			StartNodeData startNodeData = new StartNodeData();
			startNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			return startNodeData;
		}

		EventNodeData SaveNodeData(EventNode node)
		{
			EventNodeData eventNodeData = new EventNodeData(node.DialogueEvent);
			eventNodeData.SetNode(node.NodeGuid, node.GetPosition().position);

			return eventNodeData;
		}

		EndNodeData SaveNodeData(EndNode node)
		{
			EndNodeData endNodeData = new EndNodeData(node.EndNodeType);
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
				dialogueNode.SetNodeGuid(node.NodeGuid);
				dialogueNode.LoadDialogueNode(node);

				graphView.AddElement(dialogueNode);
			}

			foreach (EventNodeData node in dialogueContainerSO.EventNodeDataList)
			{
				EventNode eventNode = graphView.CreateEventNode(node.Position);
				eventNode.SetNodeGuid(node.NodeGuid);
				eventNode.SetDialogueEventSO(node.DialogueEventSO);

				graphView.AddElement(eventNode);
			}

			foreach (EndNodeData node in dialogueContainerSO.EndNodeDataList)
			{
				EndNode endNode = graphView.CreateEndNode(node.Position);
				endNode.SetNodeGuid(node.NodeGuid);
				endNode.SetEndNodeType(node.EndNodeType);

				graphView.AddElement(endNode);
			}
		}

		void ConnectNodes(DialogueContainerSO dialogueContainerSO)
		{
			foreach (BaseNode baseNode in nodeList)
			{
				List<NodeLinkData> connectionList = dialogueContainerSO.NodeLinkDataList.Where(edge => edge.BaseNodeGuid == baseNode.NodeGuid).ToList();

				foreach (NodeLinkData nodeLinkData in connectionList)
				{
					string targetNodeGuid = nodeLinkData.TargetNodeGuid;
					BaseNode targetNode = nodeList.First(node => node.NodeGuid == targetNodeGuid);

					if (!(baseNode is DialogueNode))
					{
						int index = connectionList.IndexOf(nodeLinkData);
						LinkNodesTogether((Port)targetNode.inputContainer[0], baseNode.outputContainer[index].Q<Port>());
					}
				}
			}

			List<DialogueNode> dialogueNodeList = nodeList.FindAll(node => node is DialogueNode).Cast<DialogueNode>().ToList();
			foreach (DialogueNode dialogueNode in dialogueNodeList)
			{
				foreach (DialogueNodePort nodePort in dialogueNode.DialogueInfo.DialogueNodePortList)
				{
					if (nodePort.InputGuid == string.Empty) continue;

					BaseNode targetNode = nodeList.First(node => node.NodeGuid == nodePort.InputGuid);
					LinkNodesTogether((Port)targetNode.inputContainer[0], nodePort.MyPort);
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