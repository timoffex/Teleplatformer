using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Written by Timofey Peshin (timoffex)
 * */
public class LevelChunk : MonoBehaviour {

	/// <summary>
	/// Entry point of the chunk. A chunk can only have a single entry point, so
	/// make your chunks with that in consideration!
	/// </summary>
	public ChunkEntryPoint entryPoint;

	/// <summary>
	/// All exit points of the chunk. A chunk can have multiple exit points, each of
	/// which is guaranteed to lead to a reachable location.
	/// </summary>
	public List<ChunkExitPoint> exitPoints;


	/// <summary>
	/// The chunk's boundaries. Calculated on Awake ().
	/// </summary>
	private Bounds chunkBounds;


	void Awake () {
		exitPoints = new List<ChunkExitPoint> ();

		entryPoint = GetComponentInChildren<ChunkEntryPoint> ();
		exitPoints.AddRange (GetComponentsInChildren<ChunkExitPoint> ());


		CalculateBounds ();
	}

	public Bounds GetBoundingBox () {
		// Update bounds center

		if (transform.hasChanged) {
			CalculateBounds ();
			transform.hasChanged = false;
		}

		return chunkBounds;
	}


	/// <summary>
	/// Calculates the chunkBounds by checking all of the colliders inside the chunk.
	/// </summary>
	private void CalculateBounds () {

		// Initialize (minX, minY) and (maxX, maxY)
		float minX = transform.position.x;
		float minY = transform.position.y;
		float maxX = minX;
		float maxY = maxX;


		// Get all colliders inside the chunk.
		var childColliders = GetComponentsInChildren<Collider2D> ();


		// For each collider inside the chunk...
		foreach (var col in childColliders) {
			// Update min/max positions accordingly

			var bounds = col.bounds;

			minX = Mathf.Min (minX, bounds.min.x);
			minY = Mathf.Min (minY, bounds.min.y);

			maxX = Mathf.Max (maxX, bounds.max.x);
			maxY = Mathf.Max (maxY, bounds.max.y);
		}


		// Calculate center and size of the box from (minX, minY) to (maxX, maxY)
		var center = new Vector3 ((minX + maxX) / 2, (minY + maxY) / 2, 0);
		var size = new Vector3 ((maxX - minX) / 2, (maxY - minY) / 2, 0);

		// Set our chunkBounds variable
		chunkBounds = new Bounds (center, size);
	}
}

