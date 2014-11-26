using UnityEngine;
using System.Collections;

public class Axe : Weapon {

	public ToolData td;
	float power;

	public Axe (GameObject parent) : base(parent)
	{
		relatedTypes = DeadlyThings.AXES;
		rayCastDistance = 2;
	}

	public override void ManualUpdate(Vector2 pivotScreenPosition, Vector2 pivotPosition){

		if (Input.GetMouseButton (0)) {
			power += td.gain;
			if (power > 100) power = 100;
		}else if(Input.GetMouseButtonUp(0)){
			if (power > 20){
				animator.SetTrigger("chop");
			}
			power = 0;

		}

		animator.SetInteger ("power", (int)power);

		
	}
	
	public override void SetWeapon(WeaponData wd){
		animator.SetTrigger (wd.type.ToString ());
		td = wd as ToolData;
	}

	override public void Init(){
		animator.enabled = true;
	}

	override public void Kill(){
		animator.enabled = false;
	}
}
