using UnityEngine;
using System.Collections;

/* Written by Timofey Peshin (timoffex)
 * */
public class TriggerActionEnableComponent : TriggerAction {

	public MonoBehaviour componentToEnable;

	public override void OnTrigger (Player p) {
		componentToEnable.enabled = true;
	}
}
