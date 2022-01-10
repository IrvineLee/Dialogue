 using UnityEngine;
 using UnityEditor;
using UnityEngine.UIElements;
using System;

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
            Action<Sprite> action = (sprite) => 
            {
                this.sprite = sprite;
                Repaint();
            };
            EditorUtilities.TexturePicker("Assets/Images/Female", action);
        }
     }
}