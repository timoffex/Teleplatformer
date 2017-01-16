using UnityEngine;
using System.Collections;
using System.Linq;

/* Written by Timofey Peshin (timoffex)
 * */
[RequireComponent (typeof (Rigidbody2D)),
	RequireComponent (typeof (Player))]
public class PlayerController : MonoBehaviour {

	public KeyCode interactionKey;


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
	/// faster the player reaches maxSpeed. This should be a positive number.
	/// </summary>
	public float speedUpForce;

	/// <summary>
	/// The force to apply to the player when slowing down. The higher this is, the
	/// faster the player can come to a stop or a slower speed. This should
	/// be a positive number.
	/// </summary>
	public float slowDownForce;

	/// <summary>
	/// The force to apply to the player for jumping.
	/// </summary>
	public float jumpForce;


	private Rigidbody2D myRigidBody;
	private Player myPlayer;

	/// <summary>
	/// The object we are interacting with currently. Null if no object.
	/// </summary>
	private InteractableObject interactingObject;

	// This is for initializing variables
	void Awake () {
		myRigidBody = GetComponent<Rigidbody2D> ();
		myPlayer = GetComponent<Player> ();
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


		// Check if the player is trying to interact with an object.
		if (Input.GetKey (interactionKey)) {
			if (interactingObject == null) {
				// If the player presses the interaction key and we're not interacting with anything,
				// see if there is an object we can interact with.

				// TODO: This is a little bit of a cheat.
				var interactionCheckLocation = myPlayer.grabRopeLocation.position;

				var potentialInteractableObjects = Physics2D.OverlapPointAll (interactionCheckLocation);

				// Find all InteractableObjects among the colliders.
				var interactableObjects = potentialInteractableObjects
				.Select ((col) => col.GetComponent<InteractableObject> ())
				.Where ((obj) => obj != null).ToList ();

				// If an InteractableObject is found...
				if (interactableObjects.Count > 0) {
					// Interact with it!
					interactingObject = interactableObjects [0];
					interactingObject.StartInteraction (myPlayer);
				}
			}
		} else if (Input.GetKeyUp (interactionKey)) {
			// If the player releases the interaction key, we should stop interacting with our object.


			// If we were interacting with an object...
			if (interactingObject != null) {
				// Stop interacting with it.
				interactingObject.StopInteraction (myPlayer);
				interactingObject = null;
			}
		}


		// Speed up to the right if speed < maxSpeed, otherwise slow down.
		SpeedUpRight ();
	}


	/// <summary>
	/// Tries to make right-ward speed match maxSpeed.
	/// </summary>
	private void SpeedUpRight () {
		var xSpeed = myRigidBody.velocity.x;

		if (xSpeed < maxSpeed) {
			// Speed up!


			// Calculate the change in speed after applying speedUpForce
			var changeInSpeed = speedUpForce * Time.fixedDeltaTime / myRigidBody.mass;

			// If there is any excess, excessChange will be more than 0
			var excessChange = Mathf.Max (0, changeInSpeed - (maxSpeed - xSpeed));


			// Modified force that will not allow for any excess speed
			var forceToApply = speedUpForce - excessChange * myRigidBody.mass / Time.fixedDeltaTime;

			// Apply our rightward force (Unity multiplies this by Time.fixedDeltaTime to get the change in momentum)
			myRigidBody.AddForce (Vector2.right * forceToApply);
		} else if (xSpeed > maxSpeed) {
			// Slow down!


			// Calculate the change in speed after applying slowDownForce
			var changeInSpeed = slowDownForce * Time.fixedDeltaTime / myRigidBody.mass;

			// If there is any excess, excessChange will be more than 0
			var excessChange = Mathf.Max (0, changeInSpeed - (maxSpeed - xSpeed));


			// Modified force that will not allow for any excess change
			var forceToApply = slowDownForce - excessChange * myRigidBody.mass / Time.fixedDeltaTime;

			// Apply our leftward force (Unity multiplies this by Time.fixedDeltaTime to get the change in momentum)
			myRigidBody.AddForce (Vector2.left * forceToApply);
		}
	}

	public bool IsGrounded () {
		return myPlayer.IsGrounded ();
	}
}
