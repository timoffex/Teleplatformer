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
		
		// Generate the beginning of a level and set the player position to
		// the entrance of the starting chunk.
		Vector3 playerPosition = levelGenerator.GenerateLevelStart (20);





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


	void Update () {
		// Generate the level a few units ahead of the player (20 units ≈ 4 seconds if the player moves at 5 u/s).
		levelGenerator.GenerateChunksUpTo (player.transform.position.x + 20);
	}
}
