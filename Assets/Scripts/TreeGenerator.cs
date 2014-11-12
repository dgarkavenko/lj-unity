using UnityEngine;
using System.Collections;

public class TreeGenerator : MonoBehaviour
{
	public int startX;
	public int endX;
	public int quantity;
	public SpriteRenderer baseTrunk;
	public ForestTree treeSource;
	static string[] layers = { "Foreground Trees", "Trees" };
	static int[] widths = { 8, 18, 28, 37 };


	void Generate(int width, int height, Vector3 position)
	{
		string sortingLayer = layers[Random.Range(0, layers.Length)];

		var tree = (ForestTree)GameObject.Instantiate(treeSource, position, Quaternion.identity);
		tree.name = "Tree " + width;

		var trunkTile = Resources.Load<Sprite>("Visual/Environment/Trees/tree_tile_w" + width);

		int resultHeight = 0;
		
		if (trunkTile != null) {
			//			var trunk = new GameObject("Trunk");
			//			trunk.transform.parent = transform;
			//			trunk.transform.localPosition = Vector3.zero;
			//			trunk.transform.localScale = Vector3.one;

			int tileHeight = (int)trunkTile.rect.height;
			int i = 1;
			
			for (int y = 0; y < height; y += tileHeight) {
				baseTrunk.sprite = trunkTile;
				baseTrunk.sortingLayerName = sortingLayer;
				
				var tile = ((SpriteRenderer)GameObject.Instantiate(baseTrunk));
				tile.name = "Trunk Tile " + i;
				tile.transform.parent = tree.trunk;
				tile.transform.localPosition = new Vector3((-width / 2) / 10f, y / 10f, 0);
				tile.transform.localScale = Vector3.one;

				resultHeight += tileHeight;

				++i;
			}
		}
		
		var treetopSprite = Resources.Load<Sprite>("Visual/Environment/Trees/treetop_" + width);

		if (treetopSprite != null) {
			var treetop = ((SpriteRenderer)GameObject.Instantiate(baseTrunk));

			treetop.name = "Treetop";
			treetop.sprite = treetopSprite;
			treetop.sortingLayerName = sortingLayer;
			treetop.sortingOrder = 1;
			treetop.transform.parent = tree.transform;
			treetop.transform.localPosition = new Vector3((-treetopSprite.rect.width / 2) / 10f, height / 10f, 0);
			treetop.transform.localScale = Vector3.one;
		}

		var collider = (BoxCollider2D)tree.collider2D;
		collider.size = new Vector2(width / 10f, resultHeight / 10f);
		collider.center = new Vector2(0, resultHeight / 20f);

//		collider.bounds
	}


	void OnEnable()
	{
		float x = startX;

		for (int i = 0; i < quantity; ++i) {
			Generate(widths[Random.Range(0, widths.Length)], Random.Range(0, 200), new Vector3(x, 0, 0));
			x += ((endX - startX) / (float)quantity) * i + Random.Range(-10, 10);
		}
	}
}
