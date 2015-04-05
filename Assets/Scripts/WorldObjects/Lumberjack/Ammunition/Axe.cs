using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class Axe : Weapon {

	public ToolData td;
	float power;

    
	RaycastHit2D hit;

	void Update(){

		if (Input.GetMouseButton (0)) {
			power += td.gain;
			if (power > 100) power = 100;
		}else if(Input.GetMouseButtonUp(0)){

				//TODO RaycastAll
				hit = Physics2D.Raycast(Lumberjack.PivotPosition - Vector2.up, new Vector2(Lumberjack.ViewDirection, 0), RayCastDistance, LayerMask.GetMask("Zombies", "Trees"));						


				if (hit.collider != null){
					var interactor = hit.collider.gameObject.GetComponent<Interactive>();


					if (interactor != null)
                        interactor.Interact(new ChopAction { power = (float)power, direction = Lumberjack.ViewDirection, point = hit.point });
				}
			
			power = 0;

		}

		Animator.SetFloat ("power", power);		
	}
	
	public override void SetWeapon(WeaponData wd){
		td = wd as ToolData;
	}

	override public void Init(){

        base.Init();
		Animator.SetLayerWeight(1,1);
	}

	override public void Kill(){

        base.Kill();
        Animator.SetLayerWeight(1, 0);
        power = 0;
        Animator.SetFloat("power", 0);		


        
	}
}
