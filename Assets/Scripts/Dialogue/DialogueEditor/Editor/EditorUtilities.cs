using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorUtilities
{
    /// <summary>
    /// Shows a texture with a label and a button to select a new image
    /// from a list of images loaded from the path specified. This allows
    /// a selection of an image from a subset of images, unlike the UnityEditor.ObjectField
    /// that pulls all images from /Assets/
    /// </summary>
    /// <param name='label'>
    /// Label to display.
    /// </param>
    /// <param name='selectedImage'>
    /// Selected image that shows in the interface.
    /// </param>
    /// <param name='yPosition'>
    /// How far down in the interface to show this tool.
    /// </param>
    /// <param name='textureFilePath'>
    /// Texture file path containing the images to load.
    /// </param>
    /// <param name='functionHandler'>
    /// The function to handle the selection of a new texture.
    /// </param>
    public static void TexturePreviewWithSelection(string label, Texture selectedImage, float yPosition,
        string textureFilePaths, ImageSelectedHandler functionHandler)
    {
        TexturePreviewWithSelection(label, selectedImage, yPosition, new string[] { textureFilePaths }, functionHandler);
    } // eo TexturePreviewWithSelection

    /// <summary>
    /// Shows a texture with a label and a button to select a new image
    /// from a list of images loaded from the paths specified. This allows
    /// a selection of an image from a subset of images, unlike the UnityEditor.ObjectField
    /// that pulls all images from /Assets/
    /// </summary>
    /// <param name='label'>
    /// Label to display.
    /// </param>
    /// <param name='selectedImage'>
    /// Selected image that shows in the interface.
    /// </param>
    /// <param name='yPosition'>
    /// How far down in the interface to show this tool.
    /// </param>
    /// <param name='textureFilePaths'>
    /// Texture file paths containing the images to load.
    /// </param>
    /// <param name='functionHandler'>
    /// The function to handle the selection of a new texture.
    /// </param>
    public static void TexturePreviewWithSelection(string label, Texture selectedImage, float yPosition,
        string[] textureFilePaths, ImageSelectedHandler functionHandler)
    {
        EditorGUILayout.BeginVertical(GUILayout.Height(125));
        {
            EditorGUILayout.LabelField(label);
            EditorGUI.DrawPreviewTexture(new Rect(50, yPosition, 100, 100), selectedImage);

            // used to center the select texture button
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Select Texture", GUILayout.MaxWidth(100)))
            {
                EditorUtilities.TexturePicker(textureFilePaths, functionHandler);
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
    } // eo TexturePreviewWithSelection

    public static void TexturePicker(string path, ImageSelectedHandler functionHandler)
    {
        EditorUtilities.TexturePicker(new string[] { path }, functionHandler);
    } // eo TexturePicker

    public static void TexturePickerSprite(string path, ImageSelectedHandlerSprite functionHandler)
    {
        EditorUtilities.TexturePickerSprite(new string[] { path }, functionHandler);
    } // eo TexturePicker

    /// <summary>
    /// Creates a window with buttons to select a new image. 
    /// </summary>
    /// <param name='paths'>
    /// Paths to load images from.
    /// </param>
    /// <param name='functionHandler'>
    /// How to handle the new image selection.
    /// </param>
    public static void TexturePicker(string[] paths, ImageSelectedHandler functionHandler)
    {
        TexturePickerEditor picker = (TexturePickerEditor)EditorWindow.GetWindow(typeof(TexturePickerEditor), true, "Texture Picker");
        picker.Setup(paths, functionHandler);
    } // eo TexturePicker

    public static void TexturePickerSprite(string[] paths, ImageSelectedHandlerSprite functionHandler)
    {
        TexturePickerEditor picker = (TexturePickerEditor)EditorWindow.GetWindow(typeof(TexturePickerEditor), true, "Texture Picker");
        picker.Setup(paths, functionHandler);
    } // eo TexturePicker
}
