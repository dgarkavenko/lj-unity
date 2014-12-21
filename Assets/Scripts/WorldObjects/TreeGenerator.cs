using UnityEngine;
using System.Collections;

public class TreeGenerator : MonoBehaviour
{
	public int startX;
	public int endX;
	public int quantity;
	public int minHeight;
	public int maxHeight;
	public int narrowestDistance;
	public int widestDistance;
	public SpriteRenderer baseTrunk;
	public ForestTree treeSource;
	static string[] layers = { "Foreground Trees", "Trees" };
	static int[] widths = { 8, 18, 28, 37 };


	void GenerateOneTree(int width, int height, Vector3 position)
	{
		string sortingLayer = layers[Random.Range(0, layers.Length)];

		var tree = (ForestTree)GameObject.Instantiate(treeSource, position, Quaternion.identity);
		tree.name = "Tree " + width;
		tree.trunkPixelWidth = width;

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

				tree.AddTile(tile);

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
			treetop.transform.localPosition = new Vector3(((int)-treetopSprite.rect.width / 2) / 10f, resultHeight / 10f, 0);
			treetop.transform.localScale = Vector3.one;
		}

		var rootsSprite = Resources.Load<Sprite>("Visual/Environment/Trees/roots_w" + width);
		
		if (rootsSprite != null) {
			var roots = ((SpriteRenderer)GameObject.Instantiate(baseTrunk));
			
			roots.name = "Roots";
			roots.sprite = rootsSprite;
			roots.sortingLayerName = sortingLayer;
			roots.sortingOrder = 1;
			roots.transform.parent = tree.transform;
			roots.transform.localPosition = new Vector3(((int)-rootsSprite.rect.width / 2) / 10f, 0, 0);
			roots.transform.localScale = Vector3.one;

			tree.roots = roots.transform;
		}

		var collider = (BoxCollider2D)tree.collider2D;
		collider.size = new Vector2(width / 10f, resultHeight / 10f);
		collider.center = new Vector2(0, resultHeight / 20f);

//		collider.bounds
	}



	void GenerateTrees()
	{
		float x = startX;
		
		for (int i = 0; i < quantity; ++i) {
			GenerateOneTree(widths[Random.Range(0, widths.Length)], Random.Range(minHeight, maxHeight), new Vector3(x, 0, 0));
			float distanceToNextTree = ((endX - startX) / (float)quantity) + Random.Range(-10, 10);
			x += Mathf.Clamp(distanceToNextTree, narrowestDistance, widestDistance);
//			Debug.Log("Generated tree " + i + ".");
//			yield return null;
		}
	}


	void OnEnable()
	{
//		StartCoroutine(GenerateTrees());
		GenerateTrees();
	}
}
