using UnityEngine;
using System.Collections;

public class BaseZombie : MonoBehaviour {


	public bool hit;

	public float cooldown = 0;

	public void OnHitFrame(){
		hit = true;
	}

	public EnemyConditions condition;
	public EnemyState state;
	public Schedule currentSchedule;

	protected Schedule stand;
	protected Schedule walk;
	protected Schedule pursuit;
	protected Schedule meleeAttack;
	protected Schedule rangedAttack;

	public int meleeAttackRange = 1;
	public float meleeAttackCooldown = 2;
	public int rangedAttackRange = 4;
	public float rangedAttackCooldown = 2;

	public int senceRange = 1;
	public int viewRange = 10;

	public bool debug;
	private EnemyConditions prevCondition;

	protected int viewDirection = 1;

	public bool worried = false;

	public Animator animator;


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
			return meleeAttack;
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
		meleeAttack = ZombieMind.Instance.StandartMeleeAttackSchedule;

		currentSchedule = stand;
		state = EnemyState.stand;

		if (animator == null) animator = GetComponentInChildren<Animator> ();
		IR.InteractionEvent += OnInteractionEvent;
		conditionsUpdateTime = conditionsRefreshRate / 60f;
	}

	//TODO DG Temporary
	void TEMP_ShowVFX(int dir, Vector2 point){

		GameObject vfx = GameObject.Instantiate(Resources.Load("Visual/VFX/Blood") as GameObject) as GameObject;
		vfx.transform.position = point;
		vfx.GetComponent<ParticleSystem>().Play();
		vfx.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Dynamic";

	}

	void OnInteractionEvent (IInteraction action)
	{
		switch (action.InteractionType) {

			case Interactive.InteractionType.gunshot:

				worried = true;	
				var gunshot = action as GunShotAction;						
				GetComponent<Rigidbody2D>().AddForce(new Vector2(gunshot.direction * gunshot.force, 0));				
				TEMP_ShowVFX(-1, gunshot.point);

			break;


			case Interactive.InteractionType.chop:
				worried = true;
				var chop = action as ChopAction;
				GetComponent<Rigidbody2D>().AddForce(new Vector2(chop.direction * 5 * chop.power, 0));
				TEMP_ShowVFX(-1, chop.point);
				

			break;
				default:
						break;
		}
	}

	public Interactive IR;
	public int conditionsRefreshRate = 10;
	protected float conditionsUpdateTime;
	protected float conditionsUpdate = 0;

	void Update(){

		conditionsUpdate += Time.deltaTime;
		cooldown -= Time.deltaTime;
		
		if (conditionsUpdate >= conditionsUpdateTime) {
			conditionsUpdate -= conditionsUpdateTime;
			UpdateConditions();
		}

		if (currentSchedule == null)
			ZombieMind.Instance.SelectNewSchedule(this);


		if (debug && currentSchedule.IsInterrupted (condition))
						Debug.Log ("Interrupted");

		if (currentSchedule.IsCompleted || currentSchedule.IsInterrupted (condition)) {
			ZombieMind.Instance.SelectNewSchedule(this);

		}

		animator.SetBool ("walk", currentSchedule.name == "Walk" || currentSchedule.name == "Pursuit");
		animator.SetInteger ("ms", (int)GetComponent<Rigidbody2D>().velocity.x);		

		currentSchedule.ManualUpdate(this);


	}



	protected virtual void UpdateConditions(){

		if (debug)
			prevCondition = condition;

		condition = EnemyConditions.can_stand;
		condition |= EnemyConditions.can_walk;
		condition |= ZombieMind.Instance.GetPositionConditions (this);

		if (debug && prevCondition != condition) {
			Debug.Log(condition);
		}
	}

}
