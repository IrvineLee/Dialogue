using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;

public class SpritePicker : TexturePickerEditor<Sprite> { }

public class Texture2DPicker : TexturePickerEditor<Texture2D> { }

public class TexturePickerEditor<T> : EditorWindow
{
	public List<T> images = new List<T>();

	/// <summary>
	/// A flag to indicate if the editor window has been setup or not.
	/// </summary>
	private bool isSetup = false;

	private Action<T> handler;

	#region Setup

	/// <summary>
	/// Attempts to setup the editor by reading in textures from specified path.
	/// </summary>
	/// <param name='path'>
	/// Path to load images from.
	/// </param>
	public void Setup(string path, Action<T> functionHandler)
	{
		string[] paths = new string[] { path };
		Setup(paths, functionHandler);
	} // eo Setup

	/// <summary>
	/// Attempts to setup the editor by reading in all textures specified
	/// by the various paths. Supports multiple paths of textures.
	/// </summary>
	/// <param name='paths'>
	/// Paths of textures to read in.
	/// </param>
	public void Setup(string[] paths, Action<T> functionHandler)
	{
		isSetup = true;
		ReadInAllTextures(paths);
		handler = functionHandler;
	} // eo Setup

	#endregion Setup

	#region GUI

	Vector2 scrollPosition = Vector2.zero;
	void OnGUI()
	{
		if (!isSetup)
			return;

		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

		DisplayImages(images);

		EditorGUILayout.EndScrollView();
	} // eo OnGUI

	void DisplayImages(List<T> imageList)
	{
		// create a button for each image loaded in, 4 buttons in width
		// calls the handler when a new image is selected.
		int counter = 0;
		foreach (T img in imageList)
		{
			if (counter % 4 == 0 || counter == 0)
				EditorGUILayout.BeginHorizontal();
			++counter;

			Texture2D texture = null;
			if ((typeof(T)) == (typeof(Texture2D)))
				texture = (Texture2D)(object)img;
			else if ((typeof(T)) == (typeof(Sprite)))
				texture = ((Sprite)(object)img).texture;

			if (GUILayout.Button(texture, GUILayout.Height(100), GUILayout.Width(100)))
			{
				// tell handler about new image, close selection window
				handler(img);

				EditorWindow.focusedWindow.Close();
			}

			if (counter % 4 == 0)
				EditorGUILayout.EndHorizontal();
		}
	}

	#endregion GUI

	#region Utility

	/// <summary>
	/// Reads the in all textures from the paths.
	/// </summary>
	/// <param name='paths'>
	/// The paths to read images from.
	/// </param>
	void ReadInAllTextures(string[] paths)
	{
		foreach (string path in paths)
		{
			string[] allFilesInPath = Directory.GetFiles(path);
			foreach (string filePath in allFilesInPath)
			{
				T obj = (T)(object)AssetDatabase.LoadAssetAtPath(filePath, typeof(T));
				if (obj is T)
				{
					images.Add(obj);
				}
			}
		}
	} // eo ReadInAllTextures

	#endregion Utility
}