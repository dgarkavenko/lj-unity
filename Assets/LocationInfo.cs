using UnityEngine;
using System.Collections;

namespace lj
{
    public class LocationInfo : MonoBehaviour
    {

        [MinMaxVec2Attribute(-1000, 1000, 1)]
        public Vector2 Size;
        public SpriteRenderer GroundTile;

        public AnimationCurve LightIntensity;
        public Gradient LightColor;
        public Gradient[] SkyColors;
        public Transform[] Entrances;
    }
}

