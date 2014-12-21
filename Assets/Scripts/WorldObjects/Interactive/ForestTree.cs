using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ForestTree : MonoBehaviour
{
	public Transform trunk;
	public List<SpriteRenderer> tiles;
	public Transform roots;

	public int woundYPixelPosition;

	public Sprite woundSprite;
	public bool wounded;
	public int[] woundDepth = new int[2];
	int maxWoundDepth;
	bool susceptibleToChops;

	public int trunkPixelWidth;

	public SpriteRenderer baseStomp;


	void Start()
	{
		susceptibleToChops = true;
		maxWoundDepth = trunkPixelWidth / 4 + 1;

		GetComponent<Interactive>().InteractionEvent += OnInteraction;
	}

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

					if (woundPixel.r == 0 && woundPixel.g == 0 && woundPixel.b == 0 && woundPixel.a == 1) {
						break;
					} else {
						tile.sprite.texture.SetPixel(woundSide == WoundSide.Left ? x : (tileWidth - x), woundYPixelPosition + y, woundPixel);
					}
				}
			}

			tile.sprite.texture.Apply(false);

			woundDepth[(int)woundSide]++;
		} else {
			Debug.LogWarning("No more pain, please!");
		}

		if (woundDepth[(int)WoundSide.Left] + woundDepth[(int)WoundSide.Right] >= maxWoundDepth) {
			OnBreak();
		}
	}

	void OnInteraction(IInteraction obj)
	{
		if (susceptibleToChops) {
			var chop = obj as ChopAction;
			
			if (chop != null) {
				if (chop.direction == 1) {
					DeepenWound(WoundSide.Left);
				} else if (chop.direction == -1) {
					DeepenWound(WoundSide.Right);
				}
			}
		}
	}


	void OnBreak()
	{
		susceptibleToChops = false;

		var tile = tiles[0];

		int breakHeight = woundYPixelPosition + woundSprite.texture.height / 2;
		int breakIndex = tile.sprite.texture.width * breakHeight;

		var stomp = new GameObject(name + " Stomp");
		stomp.layer = LayerMask.NameToLayer("Stomp");

		stomp.transform.position = transform.position;

		if (roots != null) {
			Vector3 rootsLocalPosition = roots.transform.localPosition;
			roots.transform.parent = stomp.transform;
			roots.transform.localPosition = rootsLocalPosition;
			roots.transform.localScale = Vector3.one;
		}

		{
			var trunkSprite = Utils.CloneSprite(tile.sprite);

			// erasing part of the trunk above the break point
			var texture = trunkSprite.texture;
			var pixels = texture.GetPixels32();

			for (int i = breakIndex; i < pixels.Length; ++i) {
				pixels[i] = new Color32(0, 0, 0, 0);
			}

			texture.SetPixels32(pixels);
			texture.Apply();

			var stompTrunk = ((SpriteRenderer)GameObject.Instantiate(baseStomp));

			stompTrunk.name = "Trunk";
			stompTrunk.sprite = trunkSprite;
			stompTrunk.sortingLayerName = tile.sortingLayerName;
			stompTrunk.sortingOrder = 1;
			stompTrunk.transform.parent = stomp.transform;
			stompTrunk.transform.localPosition = new Vector3((-trunkSprite.rect.width / 2) / 10f, 0, 0);
			stompTrunk.transform.localScale = Vector3.one;
		}

		// erasing part of the trunk below the break point
		{
			var texture = tile.sprite.texture;
			var pixels = texture.GetPixels32();
			
			for (int i = 0; i < breakIndex; ++i) {
				pixels[i] = new Color32(0, 0, 0, 0);
			}
			
			texture.SetPixels32(pixels);
			texture.Apply();
		}

		float breakPointXPosition = (float)woundDepth[(int)WoundSide.Left] / (float)(trunkPixelWidth / 4 + 1) - 0.5f;

		var stompCollider = stomp.AddComponent<PolygonCollider2D>();

		float unitWidth = (float)tile.sprite.bounds.size.x;
		float unitBreakHeight = (float)breakHeight / (tile.sprite.rect.width / tile.sprite.bounds.size.x);
		float woundUnitHeight = woundSprite.bounds.size.y / 2f;

		stompCollider.SetPath(0, new Vector2[] {
			new Vector2(-unitWidth * 0.5f, 0),
			new Vector2(-unitWidth * 0.5f, unitBreakHeight - woundUnitHeight / 2),
			new Vector2(unitWidth * breakPointXPosition, unitBreakHeight),
			new Vector2(unitWidth * 0.5f, unitBreakHeight - woundUnitHeight / 2),
			new Vector2(unitWidth * 0.5f, 0)
		});

		float colliderHeight = GetComponent<BoxCollider2D>().size.y;

		DestroyImmediate(collider2D);
		var treeCollider = gameObject.AddComponent<PolygonCollider2D>();

		treeCollider.SetPath(0, new Vector2[] {
			new Vector2(unitWidth * 0.5f, colliderHeight),
			new Vector2(unitWidth * 0.5f, unitBreakHeight + woundUnitHeight / 2),
			new Vector2(unitWidth * breakPointXPosition, unitBreakHeight),
			new Vector2(-unitWidth * 0.5f, unitBreakHeight + woundUnitHeight / 2),
			new Vector2(-unitWidth * 0.5f, colliderHeight)
		});

		gameObject.AddComponent<Rigidbody2D>();

		rigidbody2D.gravityScale = 0.3f;
	}
}
