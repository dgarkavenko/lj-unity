using UnityEngine;


public class Utils
{
	public static Sprite CloneSprite(Sprite sourceSprite)
	{
		var extents = sourceSprite.bounds.extents;
		var center = sourceSprite.bounds.center;
		
		var pivotPoint = new Vector2(0.5f - center.x / (2f * extents.x),
		                             0.5f - center.y / (2f * extents.y));
		
		float pixelsToUnits = sourceSprite.rect.width / sourceSprite.bounds.size.x;
		
		var texture = Object.Instantiate(sourceSprite.texture) as Texture2D;
		
		return Sprite.Create(texture, sourceSprite.rect, pivotPoint, pixelsToUnits);
	}
}
