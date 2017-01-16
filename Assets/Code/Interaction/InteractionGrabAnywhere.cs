using UnityEngine;
using System.Collections;

public class InteractionGrabAnywhere : InteractionAction {

	public Rigidbody2D whatToGrab;

	public override void Act (Player p) {
		p.GrabRope (whatToGrab, null);
	}

}
