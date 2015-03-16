using UnityEngine;
using System.Collections;


public class NiceTree : MonoBehaviour{
		
	public SpriteRenderer skirtRenderer;
	public SpriteRenderer trunkRenderer;
	public SpriteRenderer treetopRenderer;	

	public Texture2D WoundTexture;
	public int WoundTextureStages;

	public int woundWidth;
	public int woundHeight;

	public int Width;
	public bool Wounded; 

	public float MAX_HP;
	public float HP;

	public int TreeMaxWoundStage = 4;
	public PhysicsMaterial2D TreeMaterial;


	//pixels
	private int _woundY;

	void Start(){
		GetComponent<Interactive>().InteractionEvent += OnInteraction;
		Width = (int)trunkRenderer.sprite.rect.width;
		MAX_HP = HP = Width / 2f;
		//trunkRenderer.sprite = Utils.CloneSpriteUltra(trunkRenderer.sprite);






		woundWidth = WoundTexture.width / WoundTextureStages;
		woundHeight = WoundTexture.height;
		_woundY = Random.Range(2,5);
	}


	public int[] WoundDepth = new int[2];

	void Break ()
	{
		GetComponent<Rigidbody2D>().isKinematic = false;
		GetComponent<Collider2D>().enabled = false;
		
		var box = GetComponent<Collider2D>() as BoxCollider2D;
		float width = box.size.x;
		float heigh = box.size.y;

		GetComponent<Rigidbody2D>().mass = Width * heigh / 2f;

		var l2r = (WoundDepth [0]) / (float)(WoundDepth [0] + WoundDepth [1]);
		if (l2r == 0.5f)
						l2r += 0.1f;


		float cutX = Mathf.Lerp (0.9f, -0.9f, l2r) * width * 0.5f;
		int cutHeightPixels = (_woundY + woundHeight / 2);
		float cutHeight = cutHeightPixels / 10f;

		
		Destroy(GetComponent<Collider2D>());



		//TRUNK
		gameObject.layer = LayerMask.NameToLayer ("Trunk");
		var treeCollider = gameObject.AddComponent<PolygonCollider2D>();
		treeCollider.sharedMaterial = TreeMaterial;
		treeCollider.SetPath(0, new Vector2[] {
			new Vector2(width * 0.15f, heigh),
			new Vector2(width * 0.5f, cutHeight + 0.25f),
			new Vector2(cutX, cutHeight),
			new Vector2(-width * 0.5f, cutHeight + 0.25f),
			new Vector2(-width * 0.15f, heigh)
		});

		var transparent = new Color (0, 0, 0, 0);
		var colors = new Color[Width * cutHeightPixels];
		for (int i = 0; i < colors.Length; i++) {
			colors[i] = transparent;
		}

		var stompColors = trunkRenderer.sprite.texture.GetPixels(0, 0, Width, cutHeightPixels);
		trunkRenderer.sprite.texture.SetPixels(0, 0, Width, cutHeightPixels, colors);
		trunkRenderer.sprite.texture.Apply (false);
		//STOMP

		var stomp = new GameObject ("stomp");
		stomp.layer = LayerMask.NameToLayer("Stomp");
		stomp.transform.position = transform.position;

		var stompCollider = stomp.AddComponent<PolygonCollider2D>();
		stompCollider.SetPath(0, new Vector2[] {
			new Vector2(width * 0.5f, 0),
			new Vector2(width * 0.5f, cutHeight - 0.25f),
			new Vector2(cutX, cutHeight),
			new Vector2(-width * 0.5f, cutHeight - 0.25f),
			new Vector2(-width * 0.5f, 0)
		});

		var stompRenderer = stomp.AddComponent<SpriteRenderer> ();


		//TODO Make one shared texture for all stomps
		var TempTexture = new Texture2D (Width, cutHeightPixels, TextureFormat.RGBA32, false, false);
		TempTexture.filterMode = FilterMode.Point;
		TempTexture.SetPixels (stompColors);
		TempTexture.Apply (false);
		stompRenderer.sprite = Sprite.Create (TempTexture, new Rect (0, 0, Width, cutHeightPixels), new Vector2(0.5f, 0), 10);



	}

	void Wound (int direction, float power, bool isChainsaw)
	{

		if (HP <= 0) return;
		HP -= power;
	
		int dir01 = direction == 1 ? 1 : 0;			

		if (!Wounded) {
			trunkRenderer.sprite = Utils.CloneSpriteUltra(trunkRenderer.sprite);
			Wounded = true;
		}

		int before = WoundDepth [dir01];

		for (int i = TreeMaxWoundStage; i > 0; --i) {
			float treshold = MAX_HP - i * MAX_HP /(float)(TreeMaxWoundStage + 1) ;
			if (HP < treshold){
				WoundDepth [dir01] = Mathf.Min(i, TreeMaxWoundStage) - WoundDepth [dir01 == 0 ? 1 : 0];
				break;
			}
		}
		
		if (before == WoundDepth [dir01] && HP <= 0) {
			WoundDepth [dir01]++;
		}
		
		
		for (int y = 0; y < woundHeight; ++y) {
			for (int x = 0; x < woundWidth; ++x) {

				var woundPixel = WoundTexture.GetPixel(woundWidth * WoundDepth[dir01] + x, y);				
				if (woundPixel.r == 0 && woundPixel.g == 0 && woundPixel.b == 0 && woundPixel.a == 1) {
					break;
				} else {

					//TODO wtf is -1?!
					int targetX = dir01 == 1 ? x : (Width - x - 1);
					if (targetX < Width && targetX > -1)
						trunkRenderer.sprite.texture.SetPixel(targetX, _woundY + y, woundPixel);
				}
			}
		}
				
		trunkRenderer.sprite.texture.Apply(false);


		if (HP <= 0) {



			Break();
			return;
		}


	}
		
	void OnInteraction (IInteraction obj)
	{
		var chop = obj as ChopAction;		
		if (chop != null) {
			Wound(chop.direction, chop.power, false);			
		}
	}
}
