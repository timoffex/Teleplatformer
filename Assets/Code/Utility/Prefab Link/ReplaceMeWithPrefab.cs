using UnityEngine;
using System.Collections;

public class ReplaceMeWithPrefab : MonoBehaviour {

	public GameObject prefabToUse;

	void Start () {
		// Instantiate the prefabToUse in the same parent as us.
		var inst = Instantiate (prefabToUse, transform.parent) as GameObject;

		// Move it to our position.
		inst.transform.position = transform.position;


		// Destroy this object.
		Destroy (gameObject);
	}
}

