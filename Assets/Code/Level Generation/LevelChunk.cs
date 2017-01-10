using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Written by Timofey Peshin (timoffex)
 * */
[RequireComponent (typeof (Collider2D))]
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
	/// Chunk boundaries. This is used to figure out whether the chunk intersects
	/// with another chunk during level generation. It is also used for scaling
	/// the chunk outward to the right.
	/// </summary>
	private Collider2D chunkCollider;

	void Awake () {
		chunkCollider = GetComponent<Collider2D> ();

		exitPoints = new List<ChunkExitPoint> ();

		entryPoint = GetComponentInChildren<ChunkEntryPoint> ();
		exitPoints.AddRange (GetComponentsInChildren<ChunkExitPoint> ());
	}



	public Bounds GetBoundingBox () {
		return chunkCollider.bounds;
	}

	public Collider2D GetCollider () {
		return chunkCollider;
	}
}

