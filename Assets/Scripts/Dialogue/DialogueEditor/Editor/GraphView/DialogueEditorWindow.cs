using System.Collections;
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

using DialogueEditor.Runtime.Enums.Language;
using DialogueEditor.Runtime.ScriptableObjects;
using DialogueEditor.Editor.SaveLoad;

namespace DialogueEditor.Editor.GraphView
{
	public class DialogueEditorWindow : EditorWindow
	{
		DialogueContainerSO currentDialogueContainer;
		DialogueGraphView graphView;
		DialogueSaveAndLoad saveAndLoad;

		ToolbarMenu toolbarMenu;
		Label nameOfDialogueContainer;

		LanguageType languageType = LanguageType.English;

		public LanguageType LanguageType { get => languageType; }

		[OnOpenAsset(1)]
		public static bool ShowWindow(int instanceID, int line)
		{
			UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceID);

			if (obj is DialogueContainerSO)
			{
				DialogueEditorWindow window = (DialogueEditorWindow)GetWindow(typeof(DialogueEditorWindow));
				window.titleContent = new GUIContent("Dialogue Editor");
				window.currentDialogueContainer = obj as DialogueContainerSO;
				window.minSize = new Vector2(500, 250);
				window.Load();
			}

			return false;
		}

		void OnEnable()
		{
			ConstructGraphView();
			GenerateToolbar();
			Load();
		}

		void OnDisable()
		{
			rootVisualElement.Remove(graphView);
		}

		void ConstructGraphView()
		{
			graphView = new DialogueGraphView(this);
			graphView.StretchToParentSize();
			rootVisualElement.Add(graphView);

			saveAndLoad = new DialogueSaveAndLoad(graphView);
		}

		void GenerateToolbar()
		{
			StyleSheet styleSheet = Resources.Load<StyleSheet>("GraphViewStyleSheet");
			rootVisualElement.styleSheets.Add(styleSheet);

			Toolbar toolbar = new Toolbar();

			// Save button.
			Button saveBtn = new Button() { text = "Save" };
			saveBtn.clicked += () => { Save(); };
			toolbar.Add(saveBtn);

			// Load button.
			Button loadBtn = new Button() { text = "Load" };
			loadBtn.clicked += () => { Load(); };
			toolbar.Add(loadBtn);

			// Dropdown menu for languages.
			toolbarMenu = new ToolbarMenu();
			foreach (LanguageType language in Enum.GetValues(typeof(LanguageType)))
			{
				toolbarMenu.menu.AppendAction(language.ToString(), new Action<DropdownMenuAction>(x => SetLanguage(language, toolbarMenu)));
			}
			toolbar.Add(toolbarMenu);

			// Name of current DialogueContainer you have opened.
			nameOfDialogueContainer = new Label("");
			toolbar.Add(nameOfDialogueContainer);
			nameOfDialogueContainer.AddToClassList("nameOfDialogueContainer");

			rootVisualElement.Add(toolbar);
		}

		void Load()
		{
			if (currentDialogueContainer)
			{
				SetLanguage(LanguageType.English, toolbarMenu);
				nameOfDialogueContainer.text = "Name:   " + currentDialogueContainer.name;
				saveAndLoad.Load(currentDialogueContainer);
			}
		}

		void Save()
		{
			if (currentDialogueContainer)
				saveAndLoad.Save(currentDialogueContainer);
		}

		void SetLanguage(LanguageType language, ToolbarMenu toolbarMenu)
		{
			toolbarMenu.text = "Language: " + language.ToString();
			languageType = language;
			graphView.LanguageReload();
		}
	}
}
