﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


/* Written by Timofey Peshin (timoffex)
 * */
public class LevelGenerator : MonoBehaviour {

	/// <summary>
	/// This is the list of all chunks in the game. The developers should not touch this:
	/// rather, it should be updated automatically through an editor script!
	/// </summary>
	public List<LevelChunk> allKnownChunks;



	/// <summary>
	/// The chunk used to start the level. The entry point of this chunk is where the player is initially placed.
	/// </summary>
	public LevelChunk startingChunk;
	

	/// <summary>
	/// The set of right-most chunks. These are used to extend the level as the player is progressing.
	/// </summary>
	private List<LevelChunk> leafChunks;


	/// <summary>
	/// All the chunks that are currently in the level. Some entries of this list may become null
	/// as chunks get deleted. The list is sorted in ascending order by the max.x value of the chunk's
	/// bounding box.
	/// </summary>
	private List<LevelChunk> allChunksSorted;



	void Awake () {
		// Initialize variables
		leafChunks = new List<LevelChunk> ();
		allChunksSorted = new List<LevelChunk> ();
	}

	void Start () {
		GenerateLevelStart ();
	}



	/// <summary>
	/// Generates chunks that should follow the given chunk and puts them into the scene.
	/// </summary>
	/// <returns>An array with an element for each exit that is either a LevelChunk or null.</returns>
	/// <param name="chunk">The chunk for which we're generating follow-up chunks.</param>
	private LevelChunk[] GenerateChunkAncestors (LevelChunk chunk) {
		int maxTriesPerExit = 10;


		// This will be our return value.
		LevelChunk[] exitChunks = new LevelChunk[chunk.exitPoints.Count];

	
		// We want to iterate the exits in random order.
		int[] exitIndices = ShuffledIndices (chunk.exitPoints.Count);



		// For each exit...
		for (int i = 0; i < exitIndices.Length; i++) {
			int exitIdx = exitIndices [i];

			// Get the next exit we're trying to fill.
			ChunkExitPoint exit = chunk.exitPoints [exitIdx];


			// Try to put a chunk at that exit.
			LevelChunk randChunk = null;
			for (int numTries = 0; numTries < maxTriesPerExit; numTries++) {
				randChunk = allKnownChunks [Random.Range (0, allKnownChunks.Count)];


				// Instantiate the chunk.
				randChunk = Instantiate (randChunk);


				// Get the offset of the chunk's entrance from its pivot point.
				var entranceOffset = randChunk.entryPoint.transform.position - randChunk.transform.position;


				// Get the chunk's would-be position were it placed in the scene.
				var position = exit.transform.position - entranceOffset;


				// Reposition the chunk to that position.
				randChunk.transform.position = position;


				// Check if chunk collides with anything by using its collider's Cast() function.
				RaycastHit2D[] results = new RaycastHit2D[1];
				randChunk.GetCollider ().Cast (Vector2.up, results, 0.1f);


				// If there was a collision, destroy the chunk.
				if (results [0].collider != null) {
					Destroy (randChunk);
					randChunk = null;
				} else // Otherwise, go to the next exit
					break;
			}


			exitChunks [exitIdx] = randChunk;

			// Add the chunk to the list of all chunks in the game.
			int idxInSorted = allChunksSorted.FindLastIndex ((c) => c.GetBoundingBox ().max.x < randChunk.GetBoundingBox ().max.x);
			allChunksSorted.Insert (idxInSorted, randChunk);
		}



		return exitChunks;

	}


	private void GenerateChunksUpTo (float xPosition) {
		// Queue used to generate chunks breadth-first
		Queue<LevelChunk> chunksToProcess = new Queue<LevelChunk> ();

		// Enqueue all leaf chunks
		foreach (var leaf in leafChunks)
			chunksToProcess.Enqueue (leaf);

		// Clear the list of leaf chunks - it will be recreated in the following loop.
		leafChunks.Clear ();

		// While we have chunks left to process...
		while (chunksToProcess.Count > 0) {

			// Dequeue a chunk
			var nextLeaf = chunksToProcess.Dequeue ();


			// If the chunk is already placed after the x position, memorize it as a leaf and skip it
			if (nextLeaf.GetBoundingBox ().min.x > xPosition) {
				leafChunks.Add (nextLeaf);
				break;
			}

			// Generate children to the chunk
			LevelChunk[] followUps = GenerateChunkAncestors (nextLeaf);

			// Process each child
			foreach (LevelChunk ancestor in followUps)
				if (ancestor != null)
					chunksToProcess.Enqueue (ancestor);
		}
	}


	public Vector3 GenerateLevelStart () {
		var firstChunk = Instantiate (startingChunk);

		leafChunks.Add (firstChunk);
		allChunksSorted.Add (firstChunk);

		GenerateChunksUpTo (10);

		return firstChunk.entryPoint.transform.position;
	}



	/// <summary>
	/// Returns a shuffled list of indices from 0 up to count-1. Used to iterate
	/// an array in random order without repetition.
	/// </summary>
	/// <returns>The indices.</returns>
	/// <param name="count">Count.</param>
	private int[] ShuffledIndices (int count) {
		int[] indices = new int[count];

		for (int i = 0; i < count; i++)
			indices [i] = i;


		for (int i = 0; i < count; i++) {
			int randIdx = i + Random.Range (0, count - i);

			int temp = indices [i];
			indices [i] = indices [randIdx];
			indices [randIdx] = temp;
		}

		return indices;
	}

}
