using UnityEngine;
using System.Collections;


public class Generator : MonoBehaviour
{
	public SpriteRenderer source;
	public int startX;
	public int endX;


	void Start()
	{
		var spriteWidth = source.sprite.bounds.size.x;
		source.transform.localPosition = new Vector3(startX, source.transform.localPosition.y, source.transform.localPosition.z);
		var currentX = startX + spriteWidth;

		while (currentX < endX) {
			var newPosition = source.transform.localPosition;
			newPosition.x = currentX;

			var newObject = ((SpriteRenderer)GameObject.Instantiate(source)).transform;
			newObject.parent = source.transform.parent;
			newObject.localPosition = newPosition;
			newObject.localScale = Vector3.one;
			newObject.name = source.name;

			currentX += spriteWidth;
		}
	}
}
