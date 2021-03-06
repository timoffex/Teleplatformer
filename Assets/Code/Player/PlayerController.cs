﻿using UnityEngine;
using System.Collections;
using System.Linq;

/* Written by Timofey Peshin (timoffex)
 * */
[RequireComponent (typeof (Rigidbody2D)),
	RequireComponent (typeof (Player)), RequireComponent (typeof (Animator))]
public class PlayerController : MonoBehaviour {

	public KeyCode interactionKey;


	/// <summary>
	/// Minimum number of seconds that must pass between jumps.
	/// </summary>
	public float jumpDelay;

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
	/// The force to apply to the player when speeding up in mid-air.
	/// </summary>
	public float airSpeedUpForce;

	/// <summary>
	/// The force to apply to the player when slowing down in mid-air.
	/// </summary>
	public float airSlowDownForce;



	/// <summary>
	/// The intended speed of the player in units per second.
	/// </summary>
	private float targetSpeed;


	private Rigidbody2D myRigidBody;
	private Player myPlayer;
	private Animator myAnimator;

	/// <summary>
	/// The object we are interacting with currently. Null if no object.
	/// </summary>
	private InteractableObject interactingObject;


	/// <summary>
	/// Reference to the jumping coroutine. Null if the coroutine is done.
	/// </summary>
	private Coroutine jumpCoroutine;


	// This is for initializing variables
	void Awake () {
		myRigidBody = GetComponent<Rigidbody2D> ();
		myPlayer = GetComponent<Player> ();
		myAnimator = GetComponent<Animator> ();
	}
	
	// FixedUpdate is called once per frame and is synced with the physics system.
	void FixedUpdate () {

		// Set up target speed based on whether the player is pressing the right key.
		if (Input.GetKey (KeyCode.RightArrow))
			targetSpeed = myPlayer.runSpeed;
		else
			targetSpeed = 0;


		// If we're on the ground...
		if (IsGrounded ()) {

			// If we were jumping...
			if (jumpCoroutine != null) {

				// Stop jumping! We're on the ground already.
				myAnimator.SetTrigger ("Landed");
				Debug.Log ("Landed");
				StopCoroutine (jumpCoroutine);
				jumpCoroutine = null;
			}


			// If we want to jump...
			if (IsPressingJumpKey ()) {
				// Then start the jumping process!
				myAnimator.SetTrigger ("Jumping");
				Debug.Log ("Jumping");
				jumpCoroutine = StartCoroutine (JumpingProcess ());
			}

			// Speed up to the right if speed < maxSpeed, otherwise slow down.
			SpeedUpRight (speedUpForce, slowDownForce);
		} else {
			// Change speed using air accelerating forces.
			SpeedUpRight (airSpeedUpForce, airSlowDownForce);
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
	}


	/// <summary>
	/// Tries to make right-ward speed match target speed.
	/// </summary>
	private void SpeedUpRight (float speedUpForce, float slowDownForce) {
		var xSpeed = myRigidBody.velocity.x;

		if (xSpeed < targetSpeed) {
			// Speed up!


			// Calculate the change in speed after applying speedUpForce
			var changeInSpeed = speedUpForce * Time.fixedDeltaTime / myRigidBody.mass;

			// If there is any excess, excessChange will be more than 0
			var excessChange = Mathf.Max (0, changeInSpeed - (targetSpeed - xSpeed));


			// Modified force that will not allow for any excess speed
			float forceToApply = speedUpForce - excessChange * myRigidBody.mass / Time.fixedDeltaTime;

			// Apply our rightward force (Unity multiplies this by Time.fixedDeltaTime to get the change in momentum)
			myRigidBody.AddForce (Vector2.right * forceToApply);
		} else if (xSpeed > targetSpeed) {
			// Slow down!


			// Calculate the change in speed after applying slowDownForce
			var changeInSpeed = slowDownForce * Time.fixedDeltaTime / myRigidBody.mass;

			// If there is any excess, excessChange will be more than 0
			var excessChange = Mathf.Max (0, changeInSpeed - (xSpeed - targetSpeed));


			// Modified force that will not allow for any excess change
			float forceToApply = slowDownForce - excessChange * myRigidBody.mass / Time.fixedDeltaTime;

			// Apply our leftward force (Unity multiplies this by Time.fixedDeltaTime to get the change in momentum)
			myRigidBody.AddForce (Vector2.left * forceToApply);
		}
	}


	/// <summary>
	/// This is the jumping coroutine. It exists mainly to control jump height.
	/// </summary>
	/// <returns>The process.</returns>
	private IEnumerator JumpingProcess () {

		// Helper variables
		float hmin = myPlayer.jumpMinHeight;
		float hmax = myPlayer.jumpMaxHeight;
		float g = Vector2.Dot (Physics2D.gravity, Vector2.down);
		float m = myRigidBody.mass;
		float ds = Time.fixedDeltaTime;


		Vector2 minImpulse = Vector2.up * m * Mathf.Sqrt (2 * g * hmin);
		Vector2 maxImpulse = Vector2.up * m * Mathf.Sqrt (2 * g * hmax);

		Vector2 deltaImpulsePerSec = (maxImpulse - minImpulse) / myPlayer.jumpIncreaseTime;
		Vector2 deltaImpulsePerUpdate = deltaImpulsePerSec * ds;


		float startTime = Time.fixedTime;


		// First, apply the force we need to reach minimum jump height.
		myRigidBody.AddForce (minImpulse / ds);


		// Wait until next physics update.
		yield return new WaitForFixedUpdate ();


		// For debugging purposes..
//		var totalImpulseApplied = Vector2.zero;

		// Then, while the jump key is being pressed and jumpIncreaseTime hasn't passed...
		while (IsPressingJumpKey () && (Time.fixedTime - startTime < myPlayer.jumpIncreaseTime)) {

			// Apply our incremental jumping force
			myRigidBody.AddForce (deltaImpulsePerUpdate / ds);

//			totalImpulseApplied += deltaImpulsePerUpdate;
//			Debug.LogFormat ("Applied {0} / {1}", totalImpulseApplied, deltaImpulsePerSec * myPlayer.jumpIncreaseTime);


			// Wait until next physics update.
			yield return new WaitForFixedUpdate ();
		}
	}

	public bool IsGrounded () {
		return myPlayer.IsGrounded ();
	}

	private bool IsPressingJumpKey () {
		return Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.UpArrow);
	}
}
