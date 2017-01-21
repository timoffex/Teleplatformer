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
			GameObjectUtility.SetParentAndAlign (powerup, selection);

		// Position the powerup at the center of the screen.
		powerup.transform.position = (Vector2)SceneView.lastActiveSceneView.camera.transform.position;

		// Select the new object.
		Selection.activeObject = powerup;

		// Make this action undo-able
		Undo.RecordObject (powerup, "Created Powerup");

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
			GameObjectUtility.SetParentAndAlign (powerupZone, selection);

		// Position the powerup zone at the center of the screen.
		powerupZone.transform.position = (Vector2)SceneView.lastActiveSceneView.camera.transform.position;

		// Select the new object.
		Selection.activeObject = powerupZone;

		// Make this action undo-able
		Undo.RecordObject (powerupZone, "Created Powerup Zone");

		// Ping it as a visual cue. -- this part doesn't always work for some reason
		EditorGUIUtility.PingObject (powerupZone);
	}
}
