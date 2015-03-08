using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class Axe : Weapon {

	public ToolData td;
	float power;

	public Axe (GameObject parent) : base(parent)
	{
		relatedTypes = DeadlyThings.AXES;
		rayCastDistance = 4;
	}

	RaycastHit2D hit;

	public override void ManualUpdate(Vector2 pivotScreenPosition, Vector2 pivotPosition){


		int intDir = Input.mousePosition.x > pivotScreenPosition.x ? 1 : -1;


		if (Input.GetMouseButton (0)) {
			power += td.gain;
			if (power > 100) power = 100;
		}else if(Input.GetMouseButtonUp(0)){
			if (power > 5){
				animator.SetTrigger("chop");

				//TODO RaycastAll
				hit = Physics2D.Raycast(pivotPosition - Vector2.up, new Vector2(intDir, 0), rayCastDistance, LayerMask.GetMask("Zombies", "Trees"));						


				if (hit.collider != null){
					var interactor = hit.collider.gameObject.GetComponent<Interactive>();


					if (interactor != null)
						interactor.Interact(new ChopAction{power = (float)power, direction = intDir, point = hit.point});
				}

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
