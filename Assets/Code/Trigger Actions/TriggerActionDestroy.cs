
using UnityEngine;
using System.Collections;

/* Written by Timofey Peshin (timoffex)
 * */

public class TriggerActionDestroy : TriggerAction {

	public GameObject thingToDestroy;

	public override void OnTrigger (Player p) {
		Destroy (thingToDestroy);
	}

}

