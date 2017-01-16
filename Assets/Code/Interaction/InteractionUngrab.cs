using UnityEngine;
using System.Collections;

public class InteractionUngrab : InteractionAction {

	public override void Act (Player p) {
		p.Ungrab ();
	}
}
