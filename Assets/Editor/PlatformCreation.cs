using UnityEngine;
using UnityEditor;
using System.Collections;


/* Written by Timofey Peshin (timoffex)
 * */
public class PlatformCreation {

	[MenuItem ("GameObject/Create Other/Platform")]
	public static void CreatePlatform () {

		// Load the default platform.
		var defaultPlatform = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/Editor/Defaults/Default Platform.prefab");

		// Instantiate the default platform.
		var platform = GameObject.Instantiate (defaultPlatform);
		platform.name = "New Platform";


		// If an object is selected, add the platform as its child.
		var selection = Selection.activeGameObject;
		if (selection != null)
			platform.transform.parent = selection.transform;

		// Ping it as a visual cue. -- this part doesn't work for some reason
		EditorGUIUtility.PingObject (platform);
	}
}
