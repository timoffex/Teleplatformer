using UnityEngine;
using UnityEditor;

using System.Collections.Generic;


public class ChunkPrefabWizard : ScriptableWizard {


	[MenuItem ("Level Editor/Save Chunk", true)]
	public static bool CheckIfChunkSelected () {

		var go = Selection.activeObject;

		if (go != null && go is GameObject) {
			return (go as GameObject).GetComponent<LevelChunk> () != null;
		} else
			return false;
	}



	[MenuItem ("Level Editor/Save Chunk")]
	public static void SaveTheChunk () {
		var wiz = ScriptableWizard.DisplayWizard<ChunkPrefabWizard> ("Save Chunk", "Save and Add To List");

		// This is guaranteed to work because of the testing function defined above this one.
		wiz.chunkToSave = (Selection.activeObject as GameObject).GetComponent<LevelChunk> ();
		wiz.chunkName = Selection.activeObject.name;
	}



	public LevelChunk chunkToSave;
	public string chunkName;



	void OnWizardUpdate () {
		helpString = "Green means this object will be linked to its prefab, and yellow means there is no prefab. Objects " +
		"linked to their prefabs will be changed when their prefabs are changed, so this is great if you want to use a " +
		"platform or an object that may be further developed later.";
	}

	void OnWizardCreate () {
		// Create the chunk placeholder object

		// First, clone our chunk
		var clonedChunk = Instantiate (chunkToSave.gameObject) as GameObject;

		// Then, recursively go through every child, and when encountering something
		// that has a prefab, replace it with a ReplaceMeWithPrefab object.
		MakePreservePrefabLinks (clonedChunk, chunkToSave.gameObject);

		// Save the cloned chunk as a prefab
		var prefab = PrefabUtility.CreatePrefab (string.Format ("Assets/Prefabs/Chunks/{0}.prefab", chunkName), clonedChunk);

		// Destroy the clone
		DestroyImmediate (clonedChunk);

		// Ping and select the prefab
		Selection.activeObject = prefab;
		EditorGUIUtility.PingObject (prefab);


		// Add the new chunk to the Level Generator
		var levGenPrefab = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/Prefabs/Level Generation/LevelGenerator.prefab");
		var levGenObj = PrefabUtility.InstantiatePrefab (levGenPrefab) as GameObject;
		levGenObj.GetComponent<LevelGenerator> ().allKnownChunks.Add (prefab.GetComponent<LevelChunk> ());
		PrefabUtility.ReplacePrefab (levGenObj, levGenPrefab);
		DestroyImmediate (levGenObj);
	}

	private void MakePreservePrefabLinks (GameObject obj, GameObject original) {


		// For each child of our object...
		for (int i = 0; i < obj.transform.childCount; i++) {
			var child = obj.transform.GetChild (i);

			var childOriginal = original.transform.GetChild (i);

			var prefab = PrefabUtility.GetPrefabParent (childOriginal);
			var hasPrefab = prefab != null && (PrefabUtility.GetPrefabType (prefab) == PrefabType.Prefab
				|| PrefabUtility.GetPrefabType (prefab) == PrefabType.DisconnectedPrefabInstance);

			// If we have a prefab...
			if (hasPrefab) {
				//... replace with a ReplaceMeWithPrefab object!


				var replacement = new GameObject ();
				var script = replacement.AddComponent<ReplaceMeWithPrefab> ();
				script.prefabToUse = AssetDatabase.LoadAssetAtPath<GameObject> (AssetDatabase.GetAssetPath (prefab));

				replacement.name = string.Format ("Link to {0}", childOriginal.name);
				replacement.transform.position = child.position;


				replacement.transform.SetParent (obj.transform);
				replacement.transform.SetSiblingIndex (i);


				DestroyImmediate (child.gameObject);
			} else
				//... otherwise, continue recursing.
				MakePreservePrefabLinks (child.gameObject, childOriginal.gameObject);
		}
	
	}


	protected override bool DrawWizardGUI () {

		// Display selected chunk script
		var newChunk = EditorGUILayout.ObjectField ("Chunk:", chunkToSave, typeof(LevelChunk), true) as LevelChunk;

		if (newChunk != chunkToSave) {
			chunkToSave = newChunk;
			return true;
		}


		// Display chunk name and new folder location
		EditorGUILayout.BeginHorizontal ();

		var nameLabel = "Will be saved to Assets/Prefabs/Chunks/";
		var labelDimensions = GUI.skin.label.CalcSize(new GUIContent(nameLabel));
		EditorGUILayout.LabelField (nameLabel, GUILayout.Width (labelDimensions.x));
		var newName = EditorGUILayout.TextField (chunkName);
		EditorGUILayout.EndHorizontal ();

		if (newName != chunkName) {
			chunkName = newName;
			return true;
		}



		// Display paths
		foreach (Transform child in chunkToSave.transform)
			DisplayPaths (child.gameObject);

		return false;
	}


	private void DisplayPaths (GameObject obj) {


		// Display path for ourselves.



		var prefab = PrefabUtility.GetPrefabParent (obj);
		var hasPrefab = prefab != null && PrefabUtility.GetPrefabType (prefab) == PrefabType.Prefab;


		// Get the full rectangle for drawing the object name.
		var fullRect = GUILayoutUtility.GetRect (0, 25, GUILayout.ExpandWidth (true));
		var indentedRect = new Rect (fullRect.x + 10 + 10 * EditorGUI.indentLevel, fullRect.y, fullRect.width, fullRect.height);


		// Save indent and set it to 0. We do this because otherwise
		// Unity indents labels in a bad way.
		var oldIndent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;



		// Set the background to green if obj has a prefab, and yellow otherwise.
		var oldBG = GUI.backgroundColor;
		GUI.backgroundColor = hasPrefab ? Color.green : Color.yellow;
		GUI.Box (indentedRect, "");
		GUI.backgroundColor = oldBG;


		// Draw the object name.
		EditorGUI.LabelField (indentedRect, obj.name);



		// If this object doesn't have a prefab...
		if (!hasPrefab) {
			// Display its children!
			EditorGUI.indentLevel = oldIndent + 1;

			foreach (Transform child in obj.transform)
				DisplayPaths (child.gameObject);
		}

		// Reset indent.
		EditorGUI.indentLevel = oldIndent;
	}

}
