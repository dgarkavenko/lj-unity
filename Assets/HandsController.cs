using UnityEngine;
using System.Collections;

public class HandsController : MonoBehaviour {



    public Sprite[] pistolFrames;
    public Transform pivot;
    private SpriteRenderer renderer;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = pistolFrames[5];
	}

    public void ManualUpdate(Vector3 pivotScreenPosition)
    {
        float rad = Mathf.Atan2(Input.mousePosition.y - pivotScreenPosition.y, Input.mousePosition.x - pivotScreenPosition.x);
        BruteRotation(Rad180(rad));
    }
	
    private float Rad180(float radian) {
            float degree = 90f + radian * Mathf.Rad2Deg;		
			return degree > 180? degree - 360 :  degree;
	}

    public void BruteRotation(float degree) 
		{
			int f = 0;
			
			if (degree < 7.5) {
				f = 0;
			}else if (degree < 22.5) {
				f = 1;
			}else if (degree < 37.5) {
				f = 2;
			}else if (degree < 52.5) {
				f = 3;
			}else if (degree < 67.5) {
				f = 4;
			}else if (degree < 82.5) {
				f = 5;
			}else if (degree < 97.5) {
				f = 6;
			}else if (degree < 112.5) {
				f = 7;
			}else if (degree < 127.5) {
				f = 8;
			}else if (degree < 142.5) {
				f = 9;
			}else if (degree < 157.5) {
				f = 10;
			}else if (degree < 172.5) {
				f = 11;
			}else
				f = 12;

            renderer.sprite = pistolFrames[f];
		}
    
	
}
