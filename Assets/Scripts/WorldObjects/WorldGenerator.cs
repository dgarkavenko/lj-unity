using UnityEngine;
using System.Collections;


public class WorldGenerator : MonoBehaviour
{
	public SpriteRenderer GroundTile;
    [MinMaxVec2Attribute(-1000,1000,1)]
	public Vector2 Size;
    public BoxCollider2D GroundCollider;
    public BoxCollider ParticlesCollider;
    public BoxCollider2D[] Limits;
    public LayerMask GroundLayerMask;

	public void Process(World world, lj.LocationInfo location)
	{

        Size = location.Size;
        GroundTile = location.GroundTile;

        world.LightIntensityByTime = location.LightIntensity;
        world.LightColorByTime = location.LightColor;
        world.SkyColors = location.SkyColors;

		/*var w = GroundTile.sprite.bounds.size.x;
        var h = GroundTile.sprite.bounds.size.y;
        var currentX = Size.x;

        GroundCollider = new GameObject("Ground").AddComponent<BoxCollider2D>();
        GroundCollider.gameObject.layer = GroundLayerMask;
        GroundCollider.transform.parent = transform;
        GroundCollider.size = new Vector2(Size.y - Size.x, h);
        GroundCollider.offset = new Vector2(0, -h / 2f);

        while (currentX < Size.y)
        {
           	var newObject = ((SpriteRenderer)GameObject.Instantiate(GroundTile)).transform;

            var offsetY = newObject.transform.position.y;

            newObject.parent = GroundCollider.transform;
            newObject.position = new Vector3(currentX, offsetY, 0);
			newObject.localScale = Vector3.one;
			newObject.name = GroundTile.name;
			currentX += w;
		}

        ParticlesCollider = new GameObject("Particles Collider").AddComponent<BoxCollider>();
        ParticlesCollider.size = new Vector3(Size.y - Size.x, h, Size.y - Size.x);
        ParticlesCollider.center = new Vector3(0, -h / 2f, 0);
        ParticlesCollider.transform.parent = GroundCollider.transform;*/
	}   
}

public class Location
{

}

