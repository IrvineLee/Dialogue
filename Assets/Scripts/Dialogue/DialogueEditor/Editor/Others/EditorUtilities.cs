using System;
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
		string textureFilePaths, Action<Texture2D> functionHandler)
	{
		TexturePreviewWithSelection(label, selectedImage, yPosition, new string[] { textureFilePaths }, functionHandler);
	} // eo TexturePreviewWithSelection

	public static void TexturePreviewWithSelection(string label, Texture selectedImage, float yPosition,
	string textureFilePaths, Action<Sprite> functionHandler)
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
		string[] textureFilePaths, Action<Texture2D> functionHandler)
	{
		Action act = () => { TexturePicker(textureFilePaths, functionHandler); };
		TexturePreviewWithSelection(label, selectedImage, yPosition, textureFilePaths, act);
	} // eo TexturePreviewWithSelection

	public static void TexturePreviewWithSelection(string label, Texture selectedImage, float yPosition,
	string[] textureFilePaths, Action<Sprite> functionHandler)
	{
		Action act = () => { TexturePicker(textureFilePaths, functionHandler); };
		TexturePreviewWithSelection(label, selectedImage, yPosition, textureFilePaths, act);
	} // eo TexturePreviewWithSelection

	public static void TexturePicker(string path, Action<Texture2D> functionHandler)
	{
		EditorUtilities.TexturePicker(new string[] { path }, functionHandler);
	} // eo TexturePicker

	public static void TexturePicker(string path, Action<Sprite> functionHandler)
	{
		EditorUtilities.TexturePicker(new string[] { path }, functionHandler);
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
	public static void TexturePicker(string[] paths, Action<Texture2D> functionHandler)
	{
		Texture2DPicker picker = (Texture2DPicker)EditorWindow.GetWindow(typeof(Texture2DPicker), true, "Texture Picker");
		picker.Setup(paths, functionHandler);
	} // eo TexturePicker

	public static void TexturePicker(string[] paths, Action<Sprite> functionHandler)
	{
		SpritePicker picker = (SpritePicker)EditorWindow.GetWindow(typeof(SpritePicker), true, "Texture Picker");
		picker.Setup(paths, functionHandler);
	} // eo TexturePicker

	static void TexturePreviewWithSelection(string label, Texture selectedImage, float yPosition,
		string[] textureFilePaths, Action functionHandler)
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
				functionHandler();
			}
			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();
	}
}
