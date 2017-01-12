using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Written by Timofey Peshin (timoffex)
 * */

/// <summary>
/// This is a container class for Player information: health, energy, abilities, etc.
/// </summary>
public class Player : MonoBehaviour {


	public KeyCode ability1Key, ability2Key;


	/// <summary>
	/// All passive abilities attached to the player.
	/// </summary>
	private List<Ability> passiveAbilities = new List<Ability> ();

	/// <summary>
	/// An array with an element for every active ability type. It is null if there is no ability for that type.
	/// </summary>
	private Ability[] activeAbilities = new Ability[System.Enum.GetNames (typeof (AbilityControlType)).Length - 1];





	/// <summary>
	/// Gives the player this ability.
	/// </summary>
	public void AddAbility (Ability a) {

		// If the ability is passive, add it to the list of passive abilities
		if (a.controlType == AbilityControlType.PASSIVE)
			passiveAbilities.Add (a);
		else // Otherwise, add it to the active ability array (replacing an old ability with the same control type)
			activeAbilities [(int)a.controlType - 1] = a;



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
}
