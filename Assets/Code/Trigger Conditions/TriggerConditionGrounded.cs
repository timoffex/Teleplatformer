using UnityEngine;
using System.Collections;

/* Written by Timofey Peshin (timoffex)
 * */

/// <summary>
/// Always returns true. Trigger zone will be triggered immediately as the player enters it.
/// </summary>
public class TriggerConditionGrounded : TriggerCondition {

	public override bool IsConditionMet (Player p) {
		return p.IsGrounded ();
	}

}

