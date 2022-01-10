 using UnityEngine;
 using UnityEditor;
using UnityEngine.UIElements;

class FolderTester: EditorWindow 
 {
   
     private Sprite sprite = null;

    [MenuItem ("Window/Folder Selection Example")]
     public static void  ShowWindow () 
     {
         EditorWindow.GetWindow(typeof(FolderTester));
     }
     
     void OnGUI () 
     {
        sprite = (Sprite)EditorGUILayout.ObjectField(
			"Select Folder",
            sprite,
			typeof(Sprite),
			false);

		if (GUILayout.Button("Edit", GUILayout.Width(50)))
		{
			ImageSelectedHandlerSprite imageSelectedHandler = (sprite) => 
            { 
                this.sprite = sprite;
                Repaint();
            };
            EditorUtilities.TexturePickerSprite("Assets/Images/Female", imageSelectedHandler);
        }

        //TexturePickerEditor.Setup("Assets/Images", imageSelectedHandler);

        //sprite = (Sprite)EditorGUILayout.ObjectField(
        //     "Select Folder",
        //     targetFolder,
        //     typeof(Sprite),
        //     false);

        //if (targetFolder != null) {
        //     EditorGUILayout.HelpBox(
        //         "Valid folder! Name: " + targetFolder.name, 
        //         MessageType.Info, 
        //         true);
        // }
        // else
        // {
        //     EditorGUILayout.HelpBox(
        //         "Not valid!", 
        //         MessageType.Warning, 
        //         true);
        // }
         
     }
}