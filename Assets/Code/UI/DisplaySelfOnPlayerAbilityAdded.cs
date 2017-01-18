using UnityEngine;


using System;
using System.Collections;
using ObserverPattern;

public class DisplaySelfOnPlayerAbilityAdded : MonoBehaviour, IObserver<Ability> { 

	/// <summary>
	/// What ability control type affects whether this object gets enabled?
	/// </summary>
	public AbilityControlType triggeringControlType;

	/// <summary>
	/// How long should this object be enabled when the player gets a new ability?
	/// </summary>
	public float displayTime;


	void Awake () {
		gameObject.SetActive (false);

		// Find the player object.
		var playerObject = GameObject.FindWithTag ("Player");

		// If the player object exists...
		if (playerObject != null)
			// ... subscribe to ability updates.
			playerObject.GetComponent<Player> ().Subscribe (this);
	}


	private IEnumerator DeactivateGameObjectAfterTime (float time) {
		yield return new WaitForSeconds (time);

		gameObject.SetActive (false);
	}



	/* Observer pattern methods */
	public void OnCompleted () {}				// Ignore this.
	public void OnError (Exception error) {}	// Ignore this.

	/// <summary>
	/// Called by Player when a new ability is added.
	/// </summary>
	public void OnNext (Ability newAbility) {
		// Check if this is an ability we care about...
		if (newAbility.controlType == triggeringControlType) {
			// Enable ourselves for a set amount of time!
			gameObject.SetActive (true);
			StartCoroutine (DeactivateGameObjectAfterTime (displayTime));
		}
	}
	/* Observer pattern methods */
}
