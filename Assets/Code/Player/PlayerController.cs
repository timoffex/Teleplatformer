using UnityEngine;
using System.Collections;
using System.Linq;

/* Written by Timofey Peshin (timoffex)
 * */
[RequireComponent (typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	/// <summary>
	/// Position this below the player where the ground-check should be done.
	/// </summary>
	public Transform ground;


	/// <summary>
	/// Minimum number of seconds that must pass between jumps.
	/// </summary>
	public float jumpDelay;
	private float lastJumpTime = 0;

	/// <summary>
	/// Maximum speed of the player in units per second.
	/// </summary>
	public float maxSpeed;

	/// <summary>
	/// The force to apply to the player when accelerating. The higher this is, the
	/// faster the player reaches maxSpeed.
	/// </summary>
	public float speedUpForce;

	/// <summary>
	/// The force to apply to the player for jumping.
	/// </summary>
	public float jumpForce;


	private Rigidbody2D myRigidBody;


	// This is for initializing variables
	void Awake () {
		myRigidBody = GetComponent<Rigidbody2D> ();
	}
	
	// FixedUpdate is called once per frame and is synced with the physics system.
	void FixedUpdate () {

		// If pressing the jump button...
		if (Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.UpArrow)) {
			
			bool isGrounded = IsGrounded ();
			bool timeGood = Time.fixedTime - lastJumpTime > jumpDelay;

			// ... and if we're on the ground && enough time has passed since the last jump
			if (isGrounded && timeGood) {
				

				// jump by applying an upward force!
				myRigidBody.AddForce (Vector2.up * jumpForce);

				// remember this as the most recent jump so that we don't jump too rapidly later
				lastJumpTime = Time.fixedTime;
			}

		}
	}


	public bool IsGrounded () {

		// Find all objects that are located at the "ground" gizmo
		var allOverlaps = Physics2D.OverlapPointAll (ground.position);

		// Return true if there if there is any non-trigger collider below us
		return allOverlaps.Any ((col) => !col.isTrigger);
	}
}
