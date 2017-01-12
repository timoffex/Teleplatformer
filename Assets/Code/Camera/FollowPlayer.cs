using UnityEngine;
using System.Collections;

/* Written by Timofey Peshin (timoffex)
 * */

[RequireComponent (typeof (Camera))]
public class FollowPlayer : MonoBehaviour {


	/// <summary>
	/// The object that should be followed.
	/// </summary>
	private Transform thePlayer;


	void Update () {
		// Follow the object!

		if (thePlayer == null) {
			var playerGO = GameObject.FindGameObjectWithTag ("Player");
			if (playerGO != null)
				thePlayer = playerGO.transform;
		}

		if (thePlayer != null)
			transform.position = new Vector3 (thePlayer.position.x, thePlayer.position.y, transform.position.z);

	}

}
