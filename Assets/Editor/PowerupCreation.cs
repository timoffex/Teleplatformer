using UnityEngine;
using UnityEditor;


/* Written by Timofey Peshin (timoffex)
 * */
public class PowerupCreation {

	[MenuItem ("GameObject/Create Other/Powerup")]
	public static void CreatePowerup () {

		// Load the default powerup.
		var defaultPowerup = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/Editor/Defaults/Default Powerup.prefab");

		// Instantiate the default powerup.
		var powerup = GameObject.Instantiate (defaultPowerup);
		powerup.name = "New Powerup";


		// If an object is selected, add the powerup as its child.
		var selection = Selection.activeGameObject;
		if (selection != null)
			powerup.transform.parent = selection.transform;

		// Position the powerup at the center of the screen.
		powerup.transform.position = (Vector2)SceneView.lastActiveSceneView.camera.transform.position;

		// Ping it as a visual cue. -- this part doesn't work for some reason
		EditorGUIUtility.PingObject (powerup);
	}

	[MenuItem ("GameObject/Create Other/Powerup Zone")]
	public static void CreatePowerupZone () {

		// Load the default powerup zone.
		var defaultPowerupZone = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/Editor/Defaults/Default Powerup Zone.prefab");

		// Instantiate the default powerup zone.
		var powerupZone = GameObject.Instantiate (defaultPowerupZone);
		powerupZone.name = "New Powerup Zone";


		// If an object is selected, add the powerup zone as its child.
		var selection = Selection.activeGameObject;
		if (selection != null)
			powerupZone.transform.parent = selection.transform;

		// Position the powerup zone at the center of the screen.
		powerupZone.transform.position = (Vector2)SceneView.lastActiveSceneView.camera.transform.position;

		// Ping it as a visual cue. -- this part doesn't always work for some reason
		EditorGUIUtility.PingObject (powerupZone);
	}
}
