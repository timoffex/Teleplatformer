using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlatformCreation {

	[MenuItem ("GameObject/Create Other/Platform")]
	public static void CreatePlatform () {

		// Load the default platform.
		var defaultPlatform = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/Editor/Defaults/Default Platform.prefab");

		// Instantiate the default platform.
		var platform = GameObject.Instantiate (defaultPlatform);
		platform.name = "New Platform";

		// Ping it as a visual cue.
		EditorGUIUtility.PingObject (platform);
	}
}
