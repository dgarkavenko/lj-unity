using UnityEngine;
using System.Collections;

public class BaseZombie : MonoBehaviour, IInteractiveObject {


	public EnemyConditions condition;
	public EnemyState state;
	public Schedule currentSchedule;

	protected Schedule stand;
	protected Schedule walk;
	protected Schedule pursuit;
	protected Schedule meleeAttack;
	protected Schedule rangedAttack;

	public int meleeAttackRange = 1;
	public int rangedAttackRange = 2;
	public int senceRange = 1;
	public int viewRange = 10;

	public bool debug;

	protected int viewDirection = 1;

	public bool worried = false;


	public int ViewDirection
	{
		get { return viewDirection; }
		set {
			
			if (viewDirection != value)
			{
				viewDirection = value;

				transform.localScale = new Vector3(value, 1, 1);
			}            
		}
	}

	public Schedule Stand {
		get {
			return stand;
		}
	}

	public Schedule Walk {
		get {
			return walk;
		}
	}

	public Schedule Pursuit {
		get {
			return pursuit;
		}
	}

	public Schedule MeleeAttack {
		get {
			return pursuit;
		}
	}

	public Schedule RangedAttack {
		get {
			return rangedAttack;
		}
	}

	void Awake(){		
		stand = ZombieMind.Instance.StandartStandSchedule;		
		walk = ZombieMind.Instance.StandartWalkSchedule;	
		pursuit = ZombieMind.Instance.StandartPursuitSchedule;

		currentSchedule = stand;
		state = EnemyState.stand;

	

		conditionsUpdateTime = conditionsRefreshRate / 60f;
	}

	
	public int conditionsRefreshRate = 10;
	protected float conditionsUpdateTime;
	protected float conditionsUpdate = 0;

	void Update(){

		conditionsUpdate += Time.deltaTime;
		
		if (conditionsUpdate >= conditionsUpdateTime) {
			conditionsUpdate -= conditionsUpdateTime;
			UpdateConditions();
		}

		if (currentSchedule == null)
			ZombieMind.Instance.SelectNewSchedule(this);
		
		if (currentSchedule.IsCompleted || currentSchedule.IsInterrupted(condition))
			ZombieMind.Instance.SelectNewSchedule(this);

		currentSchedule.ManualUpdate(this);

		if (debug) {
			Debug.Log("Schedule: " + currentSchedule.Status);
		}

	}


	public virtual void Interact(Interaction interaction, IInteractiveObject subject){

	}

	protected virtual void UpdateConditions(){
		condition = EnemyConditions.can_stand;
		condition |= EnemyConditions.can_walk;
		condition |= ZombieMind.Instance.GetPositionConditions (this);

		if (debug) {
			Debug.Log(name + " conditions: " + condition);
		}
	}

}
