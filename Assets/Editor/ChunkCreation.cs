using UnityEngine;
using UnityEditor;

/* Written by Timofey Peshin (timoffex)
 * */
public class ChunkCreation {


	[MenuItem ("GameObject/Empty Chunk", priority = 10)]
	public static void CreateChunk () {
		// Load the default platform.
		var defaultChunk = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/Editor/Defaults/Default Chunk.prefab");

		// Instantiate the default chunk.
		var chunk = GameObject.Instantiate (defaultChunk);
		chunk.name = "Empty Chunk";

		// Position the chunk at the center of the screen.
		chunk.transform.position = (Vector2)SceneView.lastActiveSceneView.camera.transform.position;


		// Make the action undo-able.
		Undo.RecordObject (chunk.gameObject, "Created Empty Chunk");
		
		// Select the new chunk.
		Selection.activeGameObject = chunk.gameObject;

		// Ping it as a visual cue.
		EditorGUIUtility.PingObject (chunk.gameObject);
	}

}