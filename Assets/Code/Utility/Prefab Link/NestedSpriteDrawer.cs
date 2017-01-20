using UnityEngine;
using System;
using System.Collections;

public class NestedSpriteDrawer {

	/// <summary>
	/// Draws the sprites in the object's hierarchy at the given position and with the given tint.
	/// 
	/// The tint is applied by multiplying the color of each sprite component-wise with the tint parameter.
	/// </summary>
	public static void Visualize2DObject (GameObject obj, Vector3 position, Func<Vector3, Vector3> worldToScreen, Color tint) {

		// Get all the SpriteRenderer components on the object.
		var spriteRenderers = obj.GetComponents<SpriteRenderer> ();

		// For each SpriteRenderer...
		foreach (var sr in spriteRenderers) {
			// ... visualize it.


			var worldMin = (sr.bounds.min - obj.transform.position + position);
			var worldMax = (sr.bounds.max - obj.transform.position + position);

			worldMin.z = 0;
			worldMax.z = 0;


			var min = worldToScreen (worldMin);
			var max = worldToScreen (worldMax);

			var region = new Rect ((Vector2) min, (Vector2) (max - min));


			Debug.LogFormat ("{0} to {1}", worldMin, worldMax);
			Debug.LogFormat ("Drawing {0} in {1}", sr, region);


			DrawSprite (region, sr.sprite, Vector4.Scale (tint, sr.color));
		}


		// Visualize each child.
		foreach (Transform child in obj.transform)
			Visualize2DObject (child.gameObject, child.localPosition + position, worldToScreen, tint);
	}



	/// <summary>
	/// Draws the given sprite in the given area.
	/// </summary>
	public static void DrawSprite (Rect area, Sprite spr, Color tint) {
		if (spr == null)
			GUI.Box (area, (Texture2D)null);
		else if (spr.rect.width == spr.texture.width)
			GUI.DrawTexture (area, spr.texture, ScaleMode.ScaleToFit);
		else {
			float width = spr.texture.width;
			float height = spr.texture.height;

			Rect sourceRect = spr.textureRect;
			Rect normalizedSourceRect = new Rect (sourceRect.x / width, sourceRect.y / height,
				sourceRect.width / width, sourceRect.height / height);
			

			Graphics.DrawTexture (area, spr.texture, normalizedSourceRect,
				(int)spr.border [0], (int)spr.border [1], (int)spr.border [2], (int)spr.border [3], tint);
		}
	}



	/// <summary>
	/// Converts a Bounds variable to a Rect variable.
	/// </summary>
	private static Rect BoundsToRect (Bounds b) {
		return new Rect (b.min, b.size);
	}
}
