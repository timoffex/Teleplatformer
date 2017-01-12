using UnityEngine;
using System.Collections;

/* Written by Timofey Peshin (timoffex)
 * */
public class TriggerActionAbility : TriggerAction {


	public Ability abilityToAdd;


	public override void OnTrigger (Player p) {
		p.AddAbility (abilityToAdd);
	}

}

