using UnityEngine;


[CreateAssetMenu (menuName = "Ability/Laser")]
public class AbilityLaser : TimedAbility {

	/// <summary>
	/// The laser beam prefab.
	/// </summary>
	public GameObject laserBeamPrefab;

	/// <summary>
	/// The offset from the player's pivot where the laser's pivot should be placed.
	/// </summary>
	public Vector2 laserBeamOffset;


	/// <summary>
	/// Creates a laser beam.
	/// </summary>
	public override void PerformAbility (Player p) {
		Instantiate (laserBeamPrefab, p.transform.position + (Vector3)laserBeamOffset, Quaternion.identity, p.transform);
	}

}
