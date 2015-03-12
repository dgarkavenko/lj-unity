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

		if (viewDirTowardsLj)
		{
			float distance = Mathf.Abs(actor.transform.position.x - ljTransform.position.x);

			actor.animator.SetBool("see_enemy", distance < actor.viewRange || distance < actor.senceRange);
			actor.animator.SetBool("can_melee", distance <= actor.meleeAttackRange);
		}
		else
		{
			actor.animator.SetBool("see_enemy", actor.worried);
			actor.animator.SetBool("can_melee", false);
		}

	}

	public Schedule StandartStandSchedule{
		get{

			Schedule shedule = new Schedule ("Stand");
			shedule.interruptors = EnemyConditions.see_enemy | EnemyConditions.can_ranged_attack | EnemyConditions.can_melee_attack;
			shedule.tasks.Add (OnInitStand);
			shedule.tasks.Add (OnStand);
			shedule.taskTimeouts = new float[]{5,5};			
			return shedule;
		}
	}

	public Schedule StandartWalkSchedule{	
		get{
			Schedule shedule = new Schedule ("Walk");
			shedule.interruptors = EnemyConditions.see_enemy | EnemyConditions.can_ranged_attack | EnemyConditions.can_melee_attack;
			shedule.tasks.Add (OnInitWalk);
			shedule.tasks.Add (OnWalk);	
			shedule.taskTimeouts = new float[]{5,5};
			return shedule;
		}
	}

	public Schedule StandartPursuitSchedule{
		get{
			Schedule shedule = new Schedule ("Pursuit");
			shedule.interruptors = EnemyConditions.can_melee_attack | EnemyConditions.can_melee_attack;
			shedule.tasks.Add (OnInitPursuit);
			shedule.tasks.Add (OnPursuit);
			shedule.taskTimeouts = new float[]{5,3};	
			return shedule;
		}
	}

	public Schedule StandartMeleeAttackSchedule{
		get{
			Schedule shedule = new Schedule ("Melee");
			shedule.tasks.Add (OnInitAttack);
			shedule.tasks.Add (OnAttack);
			shedule.tasks.Add (OnEndAttack);
			return shedule;
		}
	}

	bool OnInitAttack (BaseZombie actor)
	{
		actor.worried = true;
		actor.animator.SetTrigger ("melee");
		return true;
	}

	bool OnAttack (BaseZombie actor)
	{
		if (actor.animator.GetCurrentAnimatorStateInfo(0).IsName("backswing")) {
			actor.cooldown = 2;
			return true;
		}

		return false;
	}

	bool OnEndAttack(BaseZombie actor)
	{
		if (!actor.animator.GetCurrentAnimatorStateInfo(0).IsName("backswing")) {
			return true;
		}
		return false;
	}



	protected bool ConditionMatches(EnemyConditions c, BaseZombie actor){
		return (actor.condition & c) == c;
	}
	
	

	public bool OnInitPursuit(BaseZombie actor){

		actor.worried = false;
		return true;
	}
	
	public bool OnPursuit(BaseZombie actor){

		actor.ViewDirection = ljTransform.position.x < actor.transform.position.x ? -1 : 1;
		if (Math.Abs(ljTransform.position.x - actor.transform.position.x) > actor.meleeAttackRange) {
			//actor.rigidbody2D.velocity = new Vector2 (actor.ViewDirection * 5, 0);
			actor.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(actor.ViewDirection * 30, 0));
		}
		return false;
	}
	
	public bool OnInitStand(BaseZombie actor){

		actor.GetComponent<Rigidbody2D>().velocity = new Vector2 (0, 0);	
		actor.currentSchedule.taskTimeouts[1] = UnityEngine.Random.Range (1f, 2f);
		return true;
	}
	
	public bool OnStand(BaseZombie actor){
		return false;
	}
	
	public bool OnInitWalk(BaseZombie actor){
		actor.currentSchedule.taskTimeouts[1] = UnityEngine.Random.Range (3f, 4f);
		actor.ViewDirection = UnityEngine.Random.Range (0, 100) > 50 ? -1 : 1;
		return true;
	}
	
	public bool OnWalk(BaseZombie actor){

		//actor.rigidbody2D.velocity = new Vector2 (actor.ViewDirection * 5, 0);
		actor.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(actor.ViewDirection * 30, 0));
		return false;
	}


}


