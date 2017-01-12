using UnityEngine;

/* Written by Timofey Peshin (timoffex)
 * */
public abstract class TriggerAction : MonoBehaviour {
	/// <summary>
	/// Invoked when the action should be performed, i.e. when the trigger zone is triggered by the player.
	/// </summary>
	public abstract void OnTrigger (Player p);
}
