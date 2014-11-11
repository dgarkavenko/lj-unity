using UnityEngine;
using System.Collections;

public class HandsController : MonoBehaviour {



    public Sprite[] currentWeaponFrames;
    public Transform pivot;
    private SpriteRenderer renderer;

	private Weapon currentEquip;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<SpriteRenderer>();

		currentEquip = new Gun();
		GunData gd = new GunData();
		gd.ammo_current = 100;
		gd.ammo_max = 100;
		gd.reload_time = 2;
		gd.rate = 240;

		currentEquip.SetWeapon(gd);
	}

    public void ManualUpdate(Vector3 pivotScreenPosition, Vector3 pivotPosition)
    {
		//View update
        float rad = Mathf.Atan2(Input.mousePosition.y - pivotScreenPosition.y, Input.mousePosition.x - pivotScreenPosition.x);
        BruteRotation(Mathf.Abs(Rad180(rad)));

		if (currentEquip != null)
			currentEquip.ManualUpdate (pivotScreenPosition, pivotPosition);

    }

	void Reload ()
	{

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
			}else if (degree < 85.5) {//83.5
				f = 5;
			}else if (degree < 98.5) {//97.5
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

            renderer.sprite = currentWeaponFrames[f];
		}
    
	
}
