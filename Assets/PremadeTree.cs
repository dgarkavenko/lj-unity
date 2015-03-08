using UnityEngine;
using System.Collections;

public class PremadeTree : MonoBehaviour {

	public Sprite[] treetops;
	public Sprite[] trunks;
	public Sprite[] skirts;

	public Sprite trunk;
	public Sprite treetop;
	public Sprite skirt;


	public int size;
	public Vector2 clearence_min_max;

	public int skirt_probability = 0;
	public Vector2 skirt_clearence_min_max;
	public Vector2 skirt_overlap_min_max; 

	public void Randomize(){

		treetop = treetops [Random.Range (0, treetops.Length)];
		trunk = trunks [Random.Range (0, trunks.Length)];


		if (Random.Range (0, 100) < skirt_probability && skirts.Length > 0) {
			skirt = skirts [Random.Range (0, skirts.Length)];
		}


	}

	// Use this for initialization
	void Start () {
		Randomize ();

		GameObject go;
		SpriteRenderer trunkRenderer;
		SpriteRenderer treetopRenderer;

		go = new GameObject ("trunk");
		trunkRenderer = go.AddComponent<SpriteRenderer> ();
		trunkRenderer.sprite = trunk;

		go = new GameObject ("treetop");
		treetopRenderer = go.AddComponent<SpriteRenderer> ();
		treetopRenderer.sprite = treetop;



		treetopRenderer.sortingLayerName = trunkRenderer.sortingLayerName = "Trees";
		trunkRenderer.sortingOrder = -1;	
		treetopRenderer.sortingOrder = 1;

		treetopRenderer.transform.parent = trunkRenderer.transform.parent = this.transform;

		if (skirt != null) {


			go = new GameObject ("skirt");
			var skirtRenderer = go.AddComponent<SpriteRenderer> ();
			skirtRenderer.sprite = skirt;
			skirtRenderer.transform.parent = this.transform;
			skirtRenderer.sortingLayerName = "Trees";

			skirtRenderer.transform.localPosition = Vector3.up * Random.Range(skirt_clearence_min_max.x, skirt_clearence_min_max.y);
			treetopRenderer.transform.localPosition = skirtRenderer.transform.localPosition + Vector3.up * (skirtRenderer.sprite.rect.height - Random.Range(skirt_overlap_min_max.x, skirt_overlap_min_max.y)) / 10f;

		}else{
			treetopRenderer.transform.localPosition = Vector3.up * Random.Range(clearence_min_max.x, clearence_min_max.y);

		}

			trunkRenderer.transform.localPosition = Vector3.zero;





	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
