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
		DialogueContainerSO currentDialogueContainer;								// Current open dialogue container in dialogue editor window.
		DialogueGraphView graphView;												// Reference to GraphView Class.
		DialogueSaveAndLoad saveAndLoad;											// Reference to SaveAndLoad Class.

		ToolbarMenu languageDropDownMenu;											// Language toolbar menu in the toop of the dialogue editor window.
		Label nameOfDialogueContainer;												// Name of the current open dialogue container.
		string graphViewStyleSheet = "USS/EditorWindow/EditorWindowStyleSheet";		// Name of the graph view style sheet.

		LanguageType selectedLanguage = LanguageType.English;						// Current selected language in the dialogue editor window.

		public LanguageType SelectedLanguage { get => selectedLanguage; }

		// Callback attribute for opening an asset in Unity (e.g the callback is fired when double clicking an asset in the Project Browser).
		// Read More : https://docs.unity3d.com/ScriptReference/Callbacks.OnOpenAssetAttribute.html
		[OnOpenAsset(0)]
		public static bool ShowWindow(int instanceID, int line)
		{
			// Get the currently selected item.
			UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceID);

			// Load up the window.
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
			// Create a Dialogue Graph View.
			graphView = new DialogueGraphView(this);
			graphView.StretchToParentSize();
			rootVisualElement.Add(graphView);

			saveAndLoad = new DialogueSaveAndLoad(graphView);
		}

		/// <summary>
		/// Generate the tollbar you will see in the top left of the dialogue editor window.
		/// </summary>
		void GenerateToolbar()
		{
			// Set the style sheet.
			StyleSheet styleSheet = Resources.Load<StyleSheet>(graphViewStyleSheet);
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
			languageDropDownMenu = new ToolbarMenu();
			foreach (LanguageType language in Enum.GetValues(typeof(LanguageType)))
			{
				// Set the correct language when you select from the drop down menu.
				languageDropDownMenu.menu.AppendAction(language.ToString(), new Action<DropdownMenuAction>(x => SetLanguage(language)));
			}
			toolbar.Add(languageDropDownMenu);

			// Name of current DialogueContainer you have opened.
			nameOfDialogueContainer = new Label("");
			toolbar.Add(nameOfDialogueContainer);
			nameOfDialogueContainer.AddToClassList("nameOfDialogueContainer");

			rootVisualElement.Add(toolbar);
		}

		/// <summary>
		/// Will load in current selected dialogue container.
		/// </summary>
		void Load()
		{
			if (currentDialogueContainer)
			{
				SetLanguage(LanguageType.English);
				nameOfDialogueContainer.text = "Name:   " + currentDialogueContainer.name;
				saveAndLoad.Load(currentDialogueContainer);
			}
		}

		/// <summary>
		/// Will save in current selected dialogue container.
		/// </summary>
		void Save()
		{
			if (currentDialogueContainer)
				saveAndLoad.Save(currentDialogueContainer);
		}

		/// <summary>
		/// Will change the language in the dialogue editor window.
		/// </summary>
		/// <param name="language"> Language you wanna change to </param>
		void SetLanguage(LanguageType language)
		{
			languageDropDownMenu.text = "Language: " + language.ToString();
			selectedLanguage = language;
			graphView.ReloadLanguage();
		}
	}
}
