using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlatformProperties))]
public class Platform : MonoBehaviour, IScaleable {

	/* Contains properties of the platform that may be used in scripts. */
	private PlatformProperties properties;


	/* These fields are references to the corners and middle section of a platform. */
	public SpriteRenderer leftCorner;
	public SpriteRenderer middleSection;
	public SpriteRenderer rightCorner;

	/// <summary>
	/// The original scale of the platform. Used to compute PlatformScale.
	/// The original scale is equal to the initial scale of the middle section at the time of the platform's creation.
	/// </summary>
	private float originalScale;

	/// <summary>
	/// The current scale of the platform with respect to its original scale.
	/// </summary>
	public float PlatformScale {
		get {
			return middleSection.transform.localScale.x / originalScale;
		}
	}

	void Awake () {
		properties = GetComponent<PlatformProperties> ();

		if (middleSection != null)
			originalScale = middleSection.transform.localScale.x;
		else
			originalScale = 1;
	}

	void Start () {
		StartCoroutine (Test ());
	}

	IEnumerator Test () {
		while (true) {
			ScaleToRight (1.02f);
			yield return new WaitForSeconds (0.1f);
		}
	}


	/// <summary>
	/// Scales the platform outward from its left corner by the given factor.
	/// </summary>
	public void ScaleToRight (float factor) {
		
		// { |       |= = = = =|       | }
		//   ^       ^         ^       ^
		//  pos1    pos2      pos3    pos4
		//       ^                 ^
		//    space1             space2

		// left will neither move nor scale
		// mid will expand outward from its left side
		// right will move but not scale


		float pos1, pos2, pos3, pos4;

		pos1 = leftCorner.bounds.max.x;
		pos2 = middleSection.bounds.min.x;
		pos3 = middleSection.bounds.max.x;
		pos4 = rightCorner.bounds.min.x;


		// We want to preserve the spaces between the middle section and the corners to keep
		// them the same as the platform designer set.
		float space1, space2;
		space1 = pos2 - pos1;
		space2 = pos4 - pos3;


		// scale up the middle section along the horizontal axis
		var oldMiddleScale = middleSection.transform.localScale;
		middleSection.transform.localScale = new Vector3 (oldMiddleScale.x * factor, oldMiddleScale.y, oldMiddleScale.z);



		float newSpace1 = middleSection.bounds.min.x - pos1;
		float newSpace2 = rightCorner.bounds.min.x - middleSection.bounds.max.x;

		// move middleSection and rightCorner to return them to their original spacings
		middleSection.transform.position += new Vector3 (space1 - newSpace1, 0, 0);
		rightCorner.transform.position += new Vector3 ((space1 - newSpace1) + (space2 - newSpace2), 0, 0);

	}
}

