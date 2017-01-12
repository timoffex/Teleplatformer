using UnityEngine;
using System.Collections;

/* Written by Timofey Peshin (timoffex)
 * */
public abstract class Ability : ScriptableObject {

	/// <summary>
	/// What kind of ability is this? PASSIVE means it should run every frame
	/// and doesn't collide with other abilities. Otherwise, a player can have
	/// only one ability per control (when a new ability is acquired with the same
	/// control, this ability will be removed).
	/// </summary>
	public AbilityControlType controlType;

	/// <summary>
	/// How many seconds should pass before the ability can be cast again?
	/// </summary>
	public float fireDelay;


	/// <summary>
	/// Returns true if the ability is expired and should be removed.
	/// </summary>
	public abstract bool IsExpired ();

	/// <summary>
	/// Performs the ability when the player presses the controlKey or once per frame (if isPassive).
	/// </summary>
	public abstract void PerformAbility (Player p);
}
