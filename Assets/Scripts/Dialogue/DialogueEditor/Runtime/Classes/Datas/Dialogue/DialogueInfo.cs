using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueEditor.Runtime.Classes
{
	[Serializable]
	public class DialogueInfo
	{
		[SerializeField] List<DialogueData_BaseContainer> baseContainerList = new List<DialogueData_BaseContainer>();
		[SerializeField] List<DialogueData_Name> nameList = new List<DialogueData_Name>();
		[SerializeField] List<DialogueData_Text> textList = new List<DialogueData_Text>();
		[SerializeField] List<DialogueData_Images> imagesList = new List<DialogueData_Images>();

		public List<DialogueData_BaseContainer> BaseContainerList { get => baseContainerList; }
		public List<DialogueData_Name> NameList { get => nameList; }
		public List<DialogueData_Text> TextList { get => textList; }
		public List<DialogueData_Images> ImagesList { get => imagesList; }

		public DialogueInfo() { }

		public DialogueInfo(DialogueInfo dialogueInfo)
		{
            SetDialogueInfo(dialogueInfo);
        }

		public void SetDialogueInfo(DialogueInfo dialogueInfo)
		{
            foreach (DialogueData_BaseContainer baseContainer in dialogueInfo.BaseContainerList)
            {
                // Name
                if (baseContainer is DialogueData_Name)
                {
                    DialogueData_Name tempName = (baseContainer as DialogueData_Name);
                    DialogueData_Name tempData = new DialogueData_Name();

                    tempData.SetID(tempName.Id.Value);
                    tempData.SetCharacterName(tempName.CharacterName.Value);

                    nameList.Add(tempData);
                }

                // Text
                if (baseContainer is DialogueData_Text)
                {
                    DialogueData_Text tempText = (baseContainer as DialogueData_Text);
                    DialogueData_Text tempData = new DialogueData_Text();

                    tempData.SetID(tempText.Id.Value);
                    tempData.SetValues(tempText);

                    textList.Add(tempData);
                }

                // Images
                if (baseContainer is DialogueData_Images)
                {
                    DialogueData_Images tempImage = (baseContainer as DialogueData_Images);
                    DialogueData_Images tempData = new DialogueData_Images();

                    tempData.SetID(tempImage.Id.Value);
                    tempData.SetValues(tempImage);

                    imagesList.Add(tempData);
                }
            }
        }
	}
}