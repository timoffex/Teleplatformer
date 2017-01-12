
using UnityEngine;

/* Written by Timofey Peshin (timoffex)
 * */
public abstract class TriggerCondition : MonoBehaviour {
	/// <summary>
	/// Determines whether the condition for the trigger zone is met.
	/// </summary>
	/// <returns><c>true</c> if the trigger zone should be triggered, otherwise <c>false</c>.</returns>
	public abstract bool IsConditionMet (Player p);
}
