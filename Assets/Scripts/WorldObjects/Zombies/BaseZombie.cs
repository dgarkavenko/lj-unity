using UnityEngine;
using System.Collections;

public class BaseZombie : Actor {


	public bool hit;

	public void OnHitFrame(){
		hit = true;
	}

	public EnemyConditions condition;
	public EnemyState state;

    public float NormalMoveSpeed;

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

	public Animator Animator;


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

	public override void Awake(){

        base.Awake();

		if (Animator == null) Animator = GetComponentInChildren<Animator> ();
		IR.InteractionEvent += OnInteractionEvent;
		foreach (var zstate in Animator.GetBehaviours<ZStateBase>()) zstate.Zombie = this;
        
	}

    public void Start()
    {
        conditionsUpdateTime = conditionsRefreshRate / 60f;
        StartCoroutine(ConditionsUpdateLoop());
        
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
	

	}

    public IEnumerator ConditionsUpdateLoop()
    {
        do{
            yield return new WaitForSeconds(conditionsUpdateTime);
            ZombieMind.Instance.GetPositionConditions(this);

        }while(true);
    }

    public void HorizontalMove(int d, float MoveSpeed)
    {
        ViewDirection = d;
        _rigidbody2D.velocity = new Vector2(MoveSpeed * d, _rigidbody2D.velocity.y);
    }

  


}
