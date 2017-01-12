using UnityEngine;


[CreateAssetMenu (menuName = "Ability/Test", fileName = "Test Ability")]
public class AbilityTest : Ability {
	public string text;

	public override bool IsExpired () {
		return false;
	}

	public override void PerformAbility (Player p) {
		Debug.LogFormat ("{1} ({0})", p.gameObject, text);
	}
}
