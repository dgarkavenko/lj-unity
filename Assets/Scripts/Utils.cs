using UnityEngine;


public class Utils
{

	public static Sprite CloneSpriteUltra(Sprite sourceSprite)
	{
		int x = (int)sourceSprite.textureRect.x;
		int y = (int)sourceSprite.textureRect.y;
		int h = (int)sourceSprite.textureRect.height;
		int w = (int)sourceSprite.textureRect.width;
	
		var texture = new Texture2D(w, h, TextureFormat.RGBA32, false, false);
		texture.filterMode = FilterMode.Point;	

		texture.SetPixels (0,0,w,h,sourceSprite.texture.GetPixels (x, y, w, h));
		texture.Apply (false, false);

		var pivotX = - sourceSprite.bounds.center.x / sourceSprite.bounds.extents.x / 2 + 0.5f;
		var pivotY = - sourceSprite.bounds.center.y / sourceSprite.bounds.extents.y / 2 + 0.5f;

		return Sprite.Create (texture, new Rect(0,0,w,h), new Vector2(pivotX, pivotY), sourceSprite.pixelsPerUnit);
	}
}
