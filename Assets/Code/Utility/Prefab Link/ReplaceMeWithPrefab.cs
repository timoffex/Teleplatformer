using UnityEngine;
using System.Collections;

public class ReplaceMeWithPrefab : MonoBehaviour {

	public GameObject prefabToUse;

	void Start () {
		// Instantiate the prefabToUse in the same parent as us.
		var inst = Instantiate (prefabToUse, transform.parent) as GameObject;

		// Make its transform match ours.
		inst.transform.position = transform.position;
		inst.transform.rotation = transform.rotation;
		inst.transform.localScale = transform.localScale;


		// Destroy this object.
		Destroy (gameObject);
	}
}

