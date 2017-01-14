using UnityEngine;
using System.Collections;

public class TriggerActionMakePlatformFall : TriggerAction {

	public GameObject thePlatform;
	public float rigidBodyMass = 20;

	public override void OnTrigger (Player p) {
		var rb = thePlatform.AddComponent<Rigidbody2D> ();
		rb.mass = rigidBodyMass;
	}
}
