
using UnityEngine;


public class LaserBeamBehaviour : MonoBehaviour {

	/// <summary>
	/// The maximum distance the laser can check to destroy walls.
	/// </summary>
	public float maxDistance;


	void Start () {
		Fire ();
		RemoveSelf ();
	}


	private void Fire () {
		RaycastHit2D raycastHit = Physics2D.Raycast (transform.position, Vector2.right, maxDistance);


		if (raycastHit.collider != null) // If we hit something...
			if (raycastHit.collider.GetComponent<LaserDestroyable> () != null) // ... and if that something can be destroyed by a laser...
				Destroy (raycastHit.collider.gameObject); // ... delete it!
	}


	private void RemoveSelf () {
		Destroy (gameObject);
	}
}