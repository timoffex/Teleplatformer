using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System;
using ObserverPattern;

/* Written by Timofey Peshin (timoffex)
 * */

/// <summary>
/// This is a container class for Player information: health, energy, abilities, etc.
/// </summary>

[RequireComponent (typeof (Rigidbody2D)),
	RequireComponent (typeof (Animator))]
public class Player : MonoBehaviour, IObservable<Ability> {


	public KeyCode ability1Key, ability2Key;

	/// <summary>
	/// Position this below the player where the ground-check should be done.
	/// </summary>
	public Collider2D groundCheck;


	/// <summary>
	/// When the player grabs a rope, the physics connection (i.e. the Joint) is
	/// created at this location. "Hold Rope" animations should move hands to this position.
	/// </summary>
	public Transform grabRopeLocation;


	public float runSpeed;


	/// <summary>
	/// Minimum jump height.
	/// </summary>
	public float jumpMinHeight;

	/// <summary>
	/// Maximum jump height.
	/// </summary>
	public float jumpMaxHeight;

	/// <summary>
	/// How long should the player press the jump key to reach maximum jump height? (Seconds).
	/// </summary>
	public float jumpIncreaseTime;


	/// <summary>
	/// All passive abilities attached to the player.
	/// </summary>
	private List<Ability> passiveAbilities = new List<Ability> ();

	/// <summary>
	/// An array with an element for every active ability type. It is null if there is no ability for that type.
	/// </summary>
	private Ability[] activeAbilities = new Ability[System.Enum.GetNames (typeof (AbilityControlType)).Length - 1];



	private bool isGrabbingObject = false;
	private Joint2D grabbingJoint = null;

	private Rigidbody2D myRigidBody;
	private Animator myAnimator;



	/* Observer pattern */
	// An "observer pattern" is a code design pattern where an "observable" notifies its "observers"
	// of particular changes. Observers can subscribe to an observable, and can unsubscribe using the
	// IDisposable object the observable returns in its Subscribe() method.

	private List<IObserver<Ability>> abilityObservers = new List<IObserver<Ability>> ();

	/* Observer pattern */



	void Awake () {
		myRigidBody = GetComponent<Rigidbody2D> ();
		myAnimator = GetComponent<Animator> ();
	}



	void Update () {
		myAnimator.SetFloat ("HorizontalSpeed", myRigidBody.velocity.x / runSpeed);
		myAnimator.SetFloat ("VerticalSpeed", myRigidBody.velocity.y);
		myAnimator.SetBool ("GroundCheck", IsGrounded ());
	}



	/// <summary>
	/// Determines whether the player is on the ground.
	/// </summary>
	/// <returns><c>true</c> if the player is grounded; otherwise, <c>false</c>.</returns>
	public bool IsGrounded () {
		// Find all objects that are intersecting with the ground check
		RaycastHit2D[] raycastHits = new RaycastHit2D [10];
		int numHits = groundCheck.Cast (Vector2.down, raycastHits, 0);


		// Return true if the groundCheck intersects with a non-trigger collider that doesn't belong to the player body.
		return raycastHits.Take (numHits).Any ((hit) => !hit.collider.isTrigger && hit.collider.gameObject != gameObject);
	}




	/// <summary>
	/// Grabs the specified object as if grabbing a rope.
	/// </summary>
	/// <param name="objectToGrab">The Rigidbody to which the physics Joint will be attached.</param>
	/// <param name="whereToGrabIt">The global location where the rope should be grabbed. Can be null</param>
	public void GrabRope (Rigidbody2D objectToGrab, Vector2? whereToGrabIt) {
		// If we were grabbing something before, stop grabbing it.
		if (isGrabbingObject)
			Ungrab ();

		// Create a HingeJoint which we will use to hold on to the object.
		var hinge = gameObject.AddComponent<HingeJoint2D> ();

		// Connect the hinge to /objectToGrab/ at location /whereToGrabIt/ (or the default location if /whereToGrabIt/ is null)
		hinge.connectedBody = objectToGrab;

		if (whereToGrabIt.HasValue) {
			hinge.autoConfigureConnectedAnchor = false;
			hinge.connectedAnchor = whereToGrabIt.Value - (Vector2)objectToGrab.transform.position;
		} else
			hinge.autoConfigureConnectedAnchor = true;

		// Place the hinge at the offset of our /grabRopeLocation/
		hinge.anchor = grabRopeLocation.position - transform.position;


		// Remember that we're grabbing an object and keep a reference to the hinge
		isGrabbingObject = true;
		grabbingJoint = hinge;
	}

	/// <summary>
	/// Ungrabs whatever the player is grabbing.
	/// </summary>
	public void Ungrab () {
		Destroy (grabbingJoint);
		isGrabbingObject = false;
	}


	/// <summary>
	/// Gives the player this ability.
	/// </summary>
	public void AddAbility (Ability a) {

		// If the ability is passive, add it to the list of passive abilities
		if (a.controlType == AbilityControlType.PASSIVE)
			passiveAbilities.Add (a);
		else // Otherwise, add it to the active ability array (replacing an old ability with the same control type)
			activeAbilities [(int)a.controlType - 1] = a;


		// Notify observers of the new ability.
		abilityObservers.ForEach ((obs) => obs.OnNext (a));


		StartCoroutine (AbilityCoroutine (a));
	}


	private IEnumerator AbilityCoroutine (Ability a) {

		switch (a.controlType) {
		case AbilityControlType.PASSIVE:
			while (!a.IsExpired ()) {
				a.PerformAbility (this);

				if (a.fireDelay <= Time.fixedDeltaTime)
					yield return new WaitForFixedUpdate ();
				else
					yield return new WaitForSeconds (a.fireDelay);
			}

			break;

		case AbilityControlType.ABILITY1:
			while (!a.IsExpired ()) {
				yield return new WaitUntil (() => Input.GetKey (ability1Key) || a.IsExpired ());

				if (!a.IsExpired ())
					a.PerformAbility (this);


				if (a.fireDelay <= Time.fixedDeltaTime)
					yield return new WaitForFixedUpdate ();
				else
					yield return new WaitForSeconds (a.fireDelay);
			}
			break;

		case AbilityControlType.ABILITY2:
			while (!a.IsExpired ()) {
				yield return new WaitUntil (() => Input.GetKey (ability2Key) || a.IsExpired ());

				if (!a.IsExpired ())
					a.PerformAbility (this);


				if (a.fireDelay <= Time.fixedDeltaTime)
					yield return new WaitForFixedUpdate ();
				else
					yield return new WaitForSeconds (a.fireDelay);
			}
			break;
		}


	}



	/* Observer pattern for observing ability changes. */
	public IDisposable Subscribe (IObserver<Ability> observer) {
		abilityObservers.Add (observer);
		return new ListDisposable<IObserver<Ability>> (abilityObservers, observer);
	}
}
