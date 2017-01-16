using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Collider2D))]
public class InteractableObject : MonoBehaviour {
	public InteractionAction startAction;
	public InteractionAction stopAction;


	public void StartInteraction (Player p) {
		startAction.Act (p);
	}

	public void StopInteraction (Player p) {
		stopAction.Act (p);
	}
}
