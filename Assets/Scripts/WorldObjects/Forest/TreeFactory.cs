using UnityEngine;
using System.Collections;

public class TreeFactory : MonoBehaviour {
	public TreePreset[] Presets;
	public Branches Branches;
	public NiceTree Prefab;

	void Start(){

		GrowAt (22, 0, 300, 5, transform);
	}

	int _order = 0;

	public NiceTree[] GrowAt(int count, float x_start, float x_end, float noise, Transform parent){

		var trees = new NiceTree[count];
		float width = x_end - x_start;

		for (int i = 0; i < count; i++) {
			float dx = (x_start + width / count * i);
			trees[i] = Create();
			trees[i].transform.position = new Vector2(dx + Random.Range(-noise, noise), 0);
            trees[i].transform.parent = parent;
		}

		return trees;

	}

	public NiceTree Create(){

		var preset = Presets [Random.Range(0, Presets.Length)];
		var tree = NiceTree.Instantiate (Prefab) as NiceTree;
		tree.name = "Tree";
		float clearence;

		GameObject go;	

		go = new GameObject ("trunk");
		tree.trunkRenderer = go.AddComponent<SpriteRenderer> ();

		int trunkId = Random.Range (0, preset.trunks.Length);

		tree.trunkRenderer.sprite = preset.trunks [trunkId];
		tree.TreeMaxWoundStage = preset.trunkCutStages[trunkId];

		string sln = Random.Range (0, 100) > 50 ? "Foreground Trees" : "Trees";
	
			
		go = new GameObject ("treetop");
		tree.treetopRenderer = go.AddComponent<SpriteRenderer> ();
		tree.treetopRenderer.sprite = preset.treetops [Random.Range (0, preset.treetops.Length)];	

		tree.treetopRenderer.sortingLayerName = tree.trunkRenderer.sortingLayerName = sln;
		tree.trunkRenderer.sortingOrder = _order - 1;	
		tree.treetopRenderer.sortingOrder = _order + 1;
		
		tree.treetopRenderer.transform.parent = tree.trunkRenderer.transform.parent = tree.transform;

		//Treetop
		if (Random.Range (0, 100) < preset.skirt_probability && preset.skirts.Length > 0) {		
			
			go = new GameObject ("skirt");
			var skirtRenderer = go.AddComponent<SpriteRenderer> ();
			skirtRenderer.sprite = preset.skirts [Random.Range (0, preset.skirts.Length)];
			skirtRenderer.transform.parent = tree.transform;
			skirtRenderer.sortingLayerName = sln;
			skirtRenderer.sortingOrder = _order;

			clearence = preset.Skirt_clearence;
			
			skirtRenderer.transform.localPosition = Vector3.up * clearence;
			tree.treetopRenderer.transform.localPosition = skirtRenderer.transform.localPosition + Vector3.up * (skirtRenderer.sprite.rect.height - preset.SkirtOverlap) / 10f;
			
		}else{

			clearence = preset.Clearence;
			tree.treetopRenderer.transform.localPosition = Vector3.up * clearence;
		}
		
		tree.trunkRenderer.transform.localPosition = Vector3.zero;


		int branchesCount = (int)Random.Range(0.66f, (clearence - preset.branchClearence) / preset.branchTreshold);
		GrowBranches(-1, clearence, branchesCount, tree, preset.branchClearence);

		branchesCount = (int)Random.Range(0.66f, (clearence - preset.branchClearence) / preset.branchTreshold);
		GrowBranches(1, clearence, branchesCount, tree, preset.branchClearence);

		float height = tree.treetopRenderer.transform.localPosition.y + tree.treetopRenderer.sprite.rect.height / 10f;
		(tree.GetComponent<Collider2D>() as BoxCollider2D).size = new Vector2 (tree.trunkRenderer.sprite.rect.width / 10f, height);
		(tree.GetComponent<Collider2D>() as BoxCollider2D).offset = new Vector2 (0, height/2f);


		_order += 3;

		return tree;
	}

	void GrowBranches(int side, float clearence, int count, NiceTree tree, float min_height){

		float height = min_height + (clearence - min_height) / (float)(count + 1) + Random.Range(0f,1.2f);
		GameObject go;

		for (int i = 0; i < count; i++) {			
			var branches = side == -1? Branches.left : Branches.right;			
			go = new GameObject ("branch");
			go.AddComponent<SpriteRenderer> ().sprite = branches[Random.Range(0, branches.Length)];
			go.transform.parent = tree.transform;
			go.transform.localPosition = new Vector2(side * (tree.trunkRenderer.sprite.rect.width - Random.Range(0, 5))/ 20f, height);			
			height += clearence / (float)(count + 1) + Random.Range(-0.1f, 0.3f);
		}
	}
}


[System.Serializable]
public class Branches{

	public Sprite[] left;
	public Sprite[] right;
}


[System.Serializable]
public class TreePreset{

	public Sprite[] treetops;
	public Sprite[] trunks;
	public int[] trunkCutStages;
	public Sprite[] skirts;
	public int skirt_probability;

	public Vector2 clearence_min_max;
	public float Clearence {
		get {
			return Random.Range(clearence_min_max.x, clearence_min_max.y);
		}
	}

	public Vector2 skirt_clearence_min_max;
	public float Skirt_clearence {
		get {
			return Random.Range(skirt_clearence_min_max.x, skirt_clearence_min_max.y);
		}
	}

	public Vector2 skirt_overlap_min_max;
	public float SkirtOverlap {
		get {
			return Random.Range(skirt_overlap_min_max.x, skirt_overlap_min_max.y);
		}
	}

	public float branchTreshold;
	public float branchClearence;

}