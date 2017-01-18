using UnityEngine;
using System.Collections;

public class RemoveAfterTime : MonoBehaviour {

	/// <summary>
	/// For how many seconds should this object stay alive?
	/// </summary>
	public float aliveTime;



	void Start () {
		StartCoroutine (RemoveSelfAfterTime (aliveTime));
	}


	/// <summary>
	/// Waits time seconds and destroys self.
	/// </summary>
	private IEnumerator RemoveSelfAfterTime (float time) {
		yield return new WaitForSeconds (time);

		Destroy (gameObject);
	}

}
