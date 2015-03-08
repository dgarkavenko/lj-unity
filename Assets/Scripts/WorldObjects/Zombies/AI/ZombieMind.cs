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

	public EnemyConditions GetPositionConditions(BaseZombie actor){

		EnemyConditions conditions = EnemyConditions.can_stand;

		bool viewDirTowardsLj = ( actor.ViewDirection == 1 && actor.transform.position.x < ljTransform.position.x) || (actor.ViewDirection == -1 && actor.transform.position.x > ljTransform.position.x);

		if (viewDirTowardsLj) {
			float distance = Mathf.Abs (actor.transform.position.x - ljTransform.position.x);
			if (distance <= actor.meleeAttackRange && actor.cooldown <= 0){
				conditions = EnemyConditions.see_enemy;
				conditions |= EnemyConditions.can_melee_attack;
			}else if (distance < actor.viewRange || distance < actor.senceRange){
				conditions = EnemyConditions.see_enemy;
			}
		}

		if (actor.worried) conditions |= EnemyConditions.see_enemy;

		return conditions;
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
	
	public void SelectNewSchedule (BaseZombie actor)
	{	
		if (ConditionMatches (EnemyConditions.can_melee_attack, actor))		
		{
			actor.state = EnemyState.melee;
			actor.currentSchedule = actor.MeleeAttack;
		}
		else if (ConditionMatches(EnemyConditions.can_ranged_attack, actor))
		{
			actor.state = EnemyState.ranged;
			actor.currentSchedule = actor.RangedAttack;

		}
		else if (ConditionMatches (EnemyConditions.see_enemy, actor))
		{
			actor.state = EnemyState.pursuit;
			actor.currentSchedule = actor.Pursuit;
		}
		else if (ConditionMatches(EnemyConditions.can_walk, actor) && actor.state == EnemyState.stand)
		{
			actor.state = EnemyState.walk;
			actor.currentSchedule = actor.Walk;
		}
		else if (ConditionMatches(EnemyConditions.can_stand, actor) && (actor.state == EnemyState.walk))
		{
			actor.state = EnemyState.stand;
			actor.currentSchedule = actor.Stand;
		}else{
			actor.state = EnemyState.stand;
			actor.currentSchedule = actor.Stand;
		}


		actor.currentSchedule.Reset ();

		if (actor.debug)
						Debug.Log (actor.currentSchedule.Status + ":::" + actor.condition);	
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


