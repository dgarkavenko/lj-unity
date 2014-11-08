using UnityEngine;
using System.Collections;

public class TreeGenerator : MonoBehaviour
{
	public int startX;
	public int endX;
	public int quantity;
	public SpriteRenderer baseTrunk;
	public Tree treeSource;
	static string[] layers = { "Foreground Trees", "Trees" };
	static int[] widths = { 8, 18, 28, 37 };


	void Generate(int width, int height, Vector3 position)
	{
		string sortingLayer = layers[Random.Range(0, layers.Length)];

		var tree = (Tree)GameObject.Instantiate(treeSource, position, Quaternion.identity);

		var trunkTile = Resources.Load<Sprite>("Visual/Environment/Trees/tree_tile_w" + width);
		
		if (trunkTile != null) {
			//			var trunk = new GameObject("Trunk");
			//			trunk.transform.parent = transform;
			//			trunk.transform.localPosition = Vector3.zero;
			//			trunk.transform.localScale = Vector3.one;
			
			for (int y = 0; y < height; y += (int)trunkTile.rect.height) {
				baseTrunk.sprite = trunkTile;
				baseTrunk.sortingLayerName = sortingLayer;
				
				var tile = ((SpriteRenderer)GameObject.Instantiate(baseTrunk));
				tile.transform.parent = tree.trunk;
				tile.transform.localPosition = new Vector3((-width / 2) / 10f, y / 10f, 0);
				tile.transform.localScale = Vector3.one;
			}
		}
		
		var treetopSprite = Resources.Load<Sprite>("Visual/Environment/Trees/treetop_" + width);

		if (treetopSprite != null) {
			var treetop = ((SpriteRenderer)GameObject.Instantiate(baseTrunk));
			
			treetop.sprite = treetopSprite;
			treetop.sortingLayerName = sortingLayer;
			treetop.sortingOrder = 1;
			treetop.transform.parent = tree.transform;
			treetop.transform.localPosition = new Vector3((-treetopSprite.rect.width / 2) / 10f, height / 10f, 0);
			treetop.transform.localScale = Vector3.one;
		}
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
