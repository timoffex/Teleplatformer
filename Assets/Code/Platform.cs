using UnityEngine;
using System.Collections;


/* Written by Timofey Peshin (timoffex)
 * */
[RequireComponent (typeof (PlatformProperties))]
public class Platform : MonoBehaviour, IScaleResponder {

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

	/// <summary>
	/// Shrinks corners and expands middle section.
	/// </summary>
	/// <param name="factor">Factor.</param>
	public void OnAfterScaledBy (float factor) {
		
		// { |       |= = = = =|       | }
		//   ^       ^         ^       ^
		//  pos1    pos2      pos3    pos4
		//       ^                 ^
		//    space1             space2




		float pos1, pos2, pos3, pos4;

		pos1 = leftCorner.bounds.max.x;
		pos2 = middleSection.bounds.min.x;
		pos3 = middleSection.bounds.max.x;
		pos4 = rightCorner.bounds.min.x;


		// We want to preserve the spaces between the middle section and the corners to keep
		// them the same as the platform designer set.
		float scaledSpace1, scaledSpace2;
		scaledSpace1 = pos2 - pos1;
		scaledSpace2 = pos4 - pos3;

		float correctSpace1, correctSpace2;
		correctSpace1 = scaledSpace1 / factor;
		correctSpace2 = scaledSpace2 / factor;


		// scale up the middle section along the horizontal axis
		var oldMiddleScale = middleSection.transform.localScale;
		middleSection.transform.localScale = new Vector3 (oldMiddleScale.x * factor, oldMiddleScale.y, oldMiddleScale.z);

		// scale down the corners
		var oldLeftScale = leftCorner.transform.localScale;
		var oldRightScale = rightCorner.transform.localScale;
		leftCorner.transform.localScale = new Vector3 (oldLeftScale.x / factor, oldLeftScale.y, oldLeftScale.z);
		rightCorner.transform.localScale = new Vector3 (oldRightScale.x / factor, oldRightScale.y, oldRightScale.z);


		float newSpace1 = middleSection.bounds.min.x - pos1;
		float newSpace2 = rightCorner.bounds.min.x - middleSection.bounds.max.x;

		// move left and right corners to have correct spacing from middle section
		leftCorner.transform.position += new Vector3 (newSpace1 - correctSpace1, 0, 0);
		rightCorner.transform.position += new Vector3 (correctSpace2 - newSpace2, 0, 0);


	}
}

