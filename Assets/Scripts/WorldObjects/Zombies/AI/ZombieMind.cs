using System;
using UnityEngine;

public class ZombieMind
{
	private static ZombieMind instance;

	public static ZombieMind Instance {
		get {
			if (instance == null) instance = new ZombieMind();
			return instance;
		}
	}


	protected Transform ljTransform;


	public ZombieMind ()
	{
		ljTransform = GameObject.Find ("Lumberjack").GetComponent<Transform>();
	}

	public void GetPositionConditions(BaseZombie actor){


		bool viewDirTowardsLj = ( actor.ViewDirection == 1 && actor.transform.position.x < ljTransform.position.x) || (actor.ViewDirection == -1 && actor.transform.position.x > ljTransform.position.x);
        float distance = Mathf.Abs(actor.transform.position.x - ljTransform.position.x);

		if (viewDirTowardsLj)
		{

            actor.Animator.SetBool("see_enemy", actor.worried || distance < actor.viewRange || distance < actor.senceRange);
			actor.Animator.SetBool("can_melee", distance <= actor.meleeAttackRange);
		}
		else
		{
            actor.Animator.SetBool("see_enemy", actor.worried || distance < actor.senceRange);
			actor.Animator.SetBool("can_melee", false);
		}

	}


}


