using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DialogueEditor.Runtime.Classes.Data;
using DialogueEditor.Runtime.Classes;
using UnityEngine.Events;
using DialogueEditor.Runtime.Enums.Nodes;

namespace DialogueDisplay
{
	public class DialogueTalk : DialogueGetData
	{
		DialogueController dialogueController = null;

		AudioSource audioSource;
		DialogueNodeData currentDialogueNodeData, lastDialogueNodeData;

		void Awake()
		{
			dialogueController = FindObjectOfType<DialogueController>();
			audioSource = GetComponent<AudioSource>();
		}

		public void StartDialogue()
		{
			currentDialogueNodeData = lastDialogueNodeData = null;

			CheckNodeType(GetNextNode(DialogueContainerSO.StartNodeDataList[0]));
			dialogueController.ShowDialogue(true);
		}

		void CheckNodeType(BaseNodeData baseNodeData)
		{
			switch (baseNodeData)
			{
				case StartNodeData startNodeData:
					RunNode(startNodeData);
					break;
				case DialogueNodeData dialogueNodeData:
					RunNode(dialogueNodeData);
					break;
				case EventNodeData eventNodeData:
					RunNode(eventNodeData);
					break;
				case EndNodeData endNodeData:
					RunNode(endNodeData);
					break;
				default:
					break;
			}
		}

		void RunNode(StartNodeData nodeData)
		{
			CheckNodeType(GetNextNode(DialogueContainerSO.StartNodeDataList[0]));
		}

		void RunNode(DialogueNodeData nodeData)
		{
			if (currentDialogueNodeData != nodeData)
			{
				lastDialogueNodeData = currentDialogueNodeData;
				currentDialogueNodeData = nodeData;
			}

			dialogueController.SetText(nodeData.DialogueInfo.CharacterName, nodeData.GetTextLanguageGenericTypeRuntime());
			dialogueController.SetImage(nodeData.DialogueInfo.CharacterPotrait, nodeData.DialogueInfo.PotraitFacingDirection);
			
			MakeButtons(nodeData.DialogueInfo.DialogueNodePortList);
			audioSource.clip = nodeData.GetAudioLanguageGenericTypeRuntime();
			audioSource.Play();
		}

		void RunNode(EventNodeData nodeData)
		{
			foreach (EventScriptableObjectData scriptableObjectData in nodeData.EventScriptableObjectDataList)
			{
				if (scriptableObjectData.DialogueEventSO)
					scriptableObjectData.DialogueEventSO.RunEvent();
			}
			CheckNodeType(GetNextNode(nodeData));
		}

		void RunNode(EndNodeData nodeData)
		{
			switch (nodeData.EndNodeType)
			{
				case EndNodeType.End:
					dialogueController.ShowDialogue(false);
					break;
				case EndNodeType.Repeat:
					CheckNodeType(GetNodeByGuid(currentDialogueNodeData.NodeGuid));
					break;
				case EndNodeType.GoBack:
					CheckNodeType(GetNodeByGuid(lastDialogueNodeData.NodeGuid));
					break;
				case EndNodeType.ReturnToStart:
					CheckNodeType(GetNextNode(DialogueContainerSO.StartNodeDataList[0]));
					break;
				default:
					break;
			}
		}

		void MakeButtons(List<DialogueNodePort> nodePortList)
		{
			List<string> textList = new List<string>();
			List<UnityAction> unityActionList = new List<UnityAction>();

			foreach (DialogueNodePort nodePort in nodePortList)
			{
				textList.Add(nodePort.GetTextLanguageGenericTypeRuntime());
				UnityAction unityAction = null;
				unityAction += () => 
				{
					CheckNodeType(GetNodeByGuid(nodePort.InputGuid));
					audioSource.Stop();
				};
				unityActionList.Add(unityAction);
			}

			dialogueController.SetButtons(textList, unityActionList);
		}
	}
}