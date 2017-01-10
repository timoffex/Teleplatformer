﻿using UnityEngine;
using UnityEditor;

/* Written by Timofey Peshin (timoffex)
 * */
public class ChunkCreation {


	[MenuItem ("GameObject/Create Other/Chunk/Default Chunk")]
	public static void CreateChunk () {
		// Load the default platform.
		var defaultChunk = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/Editor/Defaults/Default Chunk.prefab");

		// Instantiate the default chunk.
		var chunk = GameObject.Instantiate (defaultChunk);
		chunk.name = "New Chunk";


		// Ping it as a visual cue. -- this part doesn't work for some reason
		EditorGUIUtility.PingObject (chunk);
	}

}