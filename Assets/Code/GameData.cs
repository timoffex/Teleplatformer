using UnityEngine;
using System.Collections;


/* Written by Timofey Peshin (timoffex)
 * */
public class GameData : MonoBehaviour {

	/// <summary>
	/// A reference to the level generator.
	/// </summary>
	public LevelGenerator levelGenerator;


	/// <summary>
	/// The player prefab to use when creating the game.
	/// </summary>
	public GameObject playerPrefab;


	/// <summary>
	/// The actual player that's in the game.
	/// </summary>
	private Transform player;


	void Start () {
		Vector3 playerPosition;
		// TODO: generate a level using levelGenerator
		// TODO: levelGenerator will return a playerPosition

		// Temporary position
		playerPosition = new Vector3 (0, 0, 0);




		GameObject playerGO;

		// If there doesn't already exist a player...
		if ((playerGO = GameObject.FindGameObjectWithTag ("Player")) == null) {
			// ... then make a new one and put it in the level.
			player = (Instantiate (playerPrefab, playerPosition, Quaternion.identity) as GameObject).transform;
		} else {
			// ... otherwise, just set the player's position properly
			player = playerGO.transform;
			player.position = playerPosition;
		}
	}
}
