using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ForestTree : MonoBehaviour
{
	public Transform trunk;
	public List<SpriteRenderer> tiles;

	public bool chosenOne;
	public int woundYPosition;

	public Sprite woundSprite;
	public bool wounded;
	public int[] woundDepth = new int[2];





	public void AddTile(SpriteRenderer tile)
	{
		tiles.Add(tile);
	}

	public enum WoundSide
	{
		Left = 0,
		Right = 1
	};

	public void DeepenWound(WoundSide woundSide)
	{
		var tile = tiles[0];

		if (!wounded) {
			tile.sprite = Utils.CloneSprite(tile.sprite);
			wounded = true;
		}

		int woundWidth = (int)woundSprite.rect.width;
		int woundHeight = (int)woundSprite.rect.height;

		if (woundWidth * (woundDepth[(int)woundSide] + 1) <= woundSprite.texture.width) {
			int tileWidth = (int)tile.sprite.rect.width;

			int width = Mathf.Min(woundWidth, tileWidth);

			for (int y = 0; y < woundHeight; ++y) {
				for (int x = 0; x < width; ++x) {
					var woundPixel = woundSprite.texture.GetPixel(woundWidth * woundDepth[(int)woundSide] + x, y);

					if (woundPixel.r == 0 && woundPixel.g == 0 && woundPixel.b == 0) {
//						break;
					} else {
						tile.sprite.texture.SetPixel(woundSide == WoundSide.Left ? x : (tileWidth - x), woundYPosition + y, woundPixel);
					}
				}
			}

			tile.sprite.texture.Apply(false);

			woundDepth[(int)woundSide]++;
		} else {
			Debug.LogWarning("No more pain, please!");
		}
	}


	void OnGUI()
	{
		if (chosenOne) {
			if (GUI.Button(new Rect(200, 200, 150, 50), "Deepen left wound")) {
				DeepenWound(WoundSide.Left);
			}

			if (GUI.Button(new Rect(200, 250, 150, 50), "Deepen right wound")) {
				DeepenWound(WoundSide.Right);
			}
		}
	}
}
