using UnityEngine;

public abstract class TimedAbility : Ability {

	/// <summary>
	/// How long should the player have the ability?
	/// </summary>
	public float abilityTime;


	/// <summary>
	/// The time the ability was added to the player.
	/// </summary>
	private float startTime;

	public override void OnBeforeAbilityAdded (Player p) {
		startTime = Time.time;
	}

	public sealed override bool IsExpired () {
		return Time.time - startTime >= abilityTime;
	}
}
