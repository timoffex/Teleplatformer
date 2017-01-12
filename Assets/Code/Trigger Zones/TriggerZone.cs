using UnityEngine;
using System.Collections.Generic;

/* Written by Timofey Peshin (timoffex)
 * */
[RequireComponent (typeof (Collider2D))]
public class TriggerZone : MonoBehaviour {

	/// <summary>
	/// What should the trigger zone do if the condition is met?
	/// </summary>
	public List<TriggerAction> actions;

	/// <summary>
	/// What is the condition under which this zone is triggered and the action should execute?
	/// </summary>
	public TriggerCondition condition;



	/* These are Unity functions that are called when the underlying collider has "Is Trigger" set to true. */
	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Player"))
			CheckPlayerTrigger (col.GetComponent<Player> ());
	}

	void OnTriggerStay2D (Collider2D col) {
		if (col.CompareTag ("Player"))
			CheckPlayerTrigger (col.GetComponent<Player> ());
	}
	/*******************************************************************************************************/

	/// <summary>
	/// Performs the action if the condition is met.
	/// </summary>
	private void CheckPlayerTrigger (Player p) {
		if (condition.IsConditionMet (p))
			foreach (var action in actions)
				action.OnTrigger (p);
	}
}

