//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18331
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System;

public class Gun : Weapon
{

	public GunData gd;
	public float recoil;
	public float cooldown;
    public int currentFrame = 0;

    public Trace trace;

	public Gun (GameObject parent) : base (parent)
	{
		relatedTypes = DeadlyThings.ANY_GUN;
		rayCastDistance = 100;

        trace = GameObject.Find("Trace").GetComponent<Trace>();
	}

	void Reload ()
	{
		throw new NotImplementedException ();
	}

	override public void ManualUpdate(Vector2 pivotScreenPosition, Vector2 pivotPosition){


        float rad = Mathf.Atan2(Input.mousePosition.y - pivotScreenPosition.y, Input.mousePosition.x - pivotScreenPosition.x);
        BruteRotation(Mathf.Abs(Rad180(rad)));

		//Recoil reduction
		if (recoil > 0) {
			recoil -= gd.recoilReduction;		
		}

		//Force Reload
		if (Input.GetKeyDown (KeyCode.R)) {
			if (gd.ammo_current < gd.ammo_max) Reload();
		}

		//Reloading process
		//TODO add shotgun style relaoding
		if (gd.reload_time_left > 0) {
			gd.reload_time_left -= Time.deltaTime;
			if (gd.reload_time_left <= 0){
				gd.ammo_current = gd.ammo_max;
			}
		}

		//Cooldown
		if (cooldown > 0) {
			cooldown-= Time.deltaTime;
		}

		//LMB Pressed or JustPressed
		if (!Input.GetMouseButton (0) && !Input.GetMouseButtonDown (0))
			return;

		//Shooting
		if (gd.mode == GunData.Mode.auto) {
			if (IsReloaded){
				if (IsReady){
					Shot(pivotScreenPosition, pivotPosition);
				}
			}else{
				//AmmunitionWithin
			}
		} else {
			if (!Input.GetMouseButtonDown(0)) return;
			if (IsReloaded){
				if (IsReady){
					Shot(pivotScreenPosition, pivotPosition);
				}
			}else{
				//AmmunitionWithin
			}
		}


	}

	RaycastHit2D hit;

	void Shot (Vector2 pivotScreenPosition, Vector2 pivotPosition)
	{

		cooldown = 60 / gd.rate;
		if (gd.reload_time_left <= 0)
			gd.ammo_current--;
		else
		{
			//WarriorWithin damage 2 lj
		}

		//TODO VFX

		float dirPolar = (float)Math.Atan2(Input.mousePosition.y - pivotScreenPosition.y, Input.mousePosition.x - pivotScreenPosition.x);
		dirPolar += (gd.dispersion + recoil) * UnityEngine.Random.Range(-1f, 1f);

        Vector2 dir = new Vector2(Mathf.Cos(dirPolar), Mathf.Sin(dirPolar));

		Vector2 origin = renderer.transform.position;
        int intDir = Input.mousePosition.x > pivotScreenPosition.x ? 1 : -1;
        origin += new Vector2(gd.gunpoints[currentFrame].x * intDir, gd.gunpoints[currentFrame].y);

		hit = Physics2D.Raycast(pivotPosition, dir, rayCastDistance, LayerMask.GetMask("Zombies"));
        if (hit.collider != null)
        {
			var IR = hit.collider.gameObject.GetComponent<Interactive>();
			if (IR != null){
				IR.Interact(new GunShotAction{
					power = UnityEngine.Random.Range(gd.damage_min, gd.damage_max),
					point = hit.point, direction = intDir,
					force = gd.force});
			}

			trace.Show(origin + dir, hit.point);

        }
        else
            trace.Show(origin + dir, pivotPosition + dir * 100);
        
	}

	public bool IsReloaded{
		get { return gd.reload_time_left <= 0; }
	}

	public bool IsReady{
		get { return cooldown <= 0; }
	}

	override public void SetWeapon(WeaponData wd){
		gd = wd as GunData;
	}

    private float Rad180(float radian)
    {
        float degree = 90f + radian * Mathf.Rad2Deg;
        return degree > 180 ? degree - 360 : degree;
    }

    public void BruteRotation(float degree)
    {
        if (degree < 7.5)
        {
            currentFrame = 12;
        }
        else if (degree < 22.5)
        {
            currentFrame = 11;
        }
        else if (degree < 37.5)
        {
            currentFrame = 10;
        }
        else if (degree < 52.5)
        {
            currentFrame = 9;
        }
        else if (degree < 67.5)
        {
            currentFrame = 8;
        }
        else if (degree < 85.5)
        {//83.5
            currentFrame = 7;
        }
        else if (degree < 98.5)
        {//97.5
            currentFrame = 6;
        }
        else if (degree < 112.5)
        {
            currentFrame = 5;
        }
        else if (degree < 127.5)
        {
            currentFrame = 4;
        }
        else if (degree < 142.5)
        {
            currentFrame = 3;
        }
        else if (degree < 157.5)
        {
            currentFrame = 2;
        }
        else if (degree < 172.5)
        {
            currentFrame = 1;
        }
        else
            currentFrame = 0;

        renderer.sprite = gd.frames[currentFrame];
    }
}


