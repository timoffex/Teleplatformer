
/* Written by Timofey Peshin (timoffex)
 * */

public interface IScaleable {
	/// <summary>
	/// Scales the object horizontally by the given factor then moves the object
	/// such that the x distance from its center to the given x position is scaled by factor.
	/// 
	/// Postcondition:
	/// 	center.x - xPosition = factor * (oldCenter.x - xPosition)
	/// 	scale.x = factor * oldScale.x
	/// </summary>
	void ScaleFromXPosition (float factor, float xPosition);
}
