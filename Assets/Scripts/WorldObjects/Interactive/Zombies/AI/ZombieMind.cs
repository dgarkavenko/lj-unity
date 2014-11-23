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

	protected Schedule stand;
	protected Schedule walk;
	protected Schedule pursuit;
	protected Schedule meleeAttack;
	protected Schedule rangedAttack;

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
			if (distance <= actor.meleeAttackRange){
				conditions = EnemyConditions.see_enemy;
				conditions |= EnemyConditions.can_melee_attack;
			}else if (distance < actor.viewRange || actor.worried || distance < actor.senceRange){
				conditions = EnemyConditions.see_enemy;
			}
		}

		return conditions;
	}

	public Schedule StandartStandSchedule{
		get{
			stand = new Schedule ("Stand");
			stand.interruptors = EnemyConditions.see_enemy | EnemyConditions.can_ranged_attack | EnemyConditions.can_melee_attack;
			stand.tasks.Add (OnInitStand);
			stand.tasks.Add (OnStand);
			stand.taskTimeouts = new float[]{5,5};			
			return stand;
		}
	}

	public Schedule StandartWalkSchedule{	
		get{
			walk = new Schedule ("Walk");
			walk.interruptors = EnemyConditions.see_enemy | EnemyConditions.can_ranged_attack | EnemyConditions.can_melee_attack;
			walk.tasks.Add (OnInitWalk);
			walk.tasks.Add (OnWalk);	
			walk.taskTimeouts = new float[]{5,5};
			return walk;
		}
	}

	public Schedule StandartPursuitSchedule{
		get{
			pursuit = new Schedule ("Pursuit");
			pursuit.interruptors = EnemyConditions.can_melee_attack | EnemyConditions.can_melee_attack;
			pursuit.tasks.Add (OnInitPursuit);
			pursuit.tasks.Add (OnPursuit);
			//pursuit.taskTimeouts = new float[]{5,2};	
			return pursuit;
		}
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
		else if (ConditionMatches(EnemyConditions.can_stand, actor) && actor.state == EnemyState.walk)
		{
			actor.state = EnemyState.stand;
			actor.currentSchedule = actor.Stand;
		}
		
		actor.currentSchedule.Reset ();
	}

	public bool OnInitPursuit(BaseZombie actor){
		actor.worried = true;
		return true;
	}
	
	public bool OnPursuit(BaseZombie actor){


		actor.ViewDirection = ljTransform.position.x < actor.transform.position.x ? -1 : 1;
		if (Math.Abs(ljTransform.position.x - actor.transform.position.x) > actor.meleeAttackRange) {
			actor.rigidbody2D.velocity = new Vector2 (actor.ViewDirection * 5, 0);
		}
		return false;
	}
	
	public bool OnInitStand(BaseZombie actor){

		//actor.view.stand();
		actor.rigidbody2D.velocity = new Vector2 (0, 0);
		actor.currentSchedule.taskTimeouts[1] = UnityEngine.Random.Range (1f, 2f);
		return true;
	}
	
	public bool OnStand(BaseZombie actor){
		return false;
	}
	
	public bool OnInitWalk(BaseZombie actor){
		actor.currentSchedule.taskTimeouts[1] = UnityEngine.Random.Range (1f, 2f);
		actor.ViewDirection = UnityEngine.Random.Range (0, 100) > 50 ? -1 : 1;
		return true;
	}
	
	public bool OnWalk(BaseZombie actor){
		actor.rigidbody2D.velocity = new Vector2 (actor.ViewDirection * 5, 0);
		return false;
	}


}


