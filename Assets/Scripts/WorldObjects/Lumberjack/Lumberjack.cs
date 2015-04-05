using UnityEngine;
using System.Collections;

public class Lumberjack : MonoBehaviour
{


	public LayerMask groundMask;
	public bool Grounded = false;
	public bool Moving = false;
	public Transform grounder;
	
    public Transform Body;
    public Transform Pivot;


    
    public Vector2 PivotPosition
    {
        get
        {
            return Pivot.position;
        }
    }
    public Vector2 PivotScreenPosition;
	public GameObject Hands;

	public Animator Animator;
	   
	private Weapon _currentEquipContainer;
	public Weapon[] AllEquipContainers;

	private Rigidbody2D _rigidbody2D;



	public float NormalMoveSpeed = 15;
	public float NormalDrag = 1;
	public float JumpDrag = 1;

	public Vector2 LiftPower;
	public Vector2 NormalJump = new Vector2(0, 450);

	// --- TEMP ---



	// --- TEMP ---


	void Start () {


        var r = Hands.GetComponent<SpriteRenderer>();
		_currentEquipContainer = AllEquipContainers [0];

		GunData gd = GameplayData.Instance.guns[0];		
		_currentEquipContainer.SetWeapon(gd);

		_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
		Animator = gameObject.GetComponent<Animator>();

		foreach (var state in Animator.GetBehaviours<LJStateBase>()) state.lj = this;


		ZStateBase.LjTransform = this.transform;

	}

	private void SelectWeapon(WeaponData wd){

		Debug.Log ("Switching to: " + wd.alias);

		foreach (var container in AllEquipContainers) {
			if((container.RelatedTypes & wd.type) == wd.type){

				if (_currentEquipContainer != container)
				{
					_currentEquipContainer.Kill();
					_currentEquipContainer = container;
					_currentEquipContainer.Init();
				}

				break;
			}
		}

		_currentEquipContainer.SetWeapon(wd);		
	}

    private void OnMovementDirectionChanged(int dir)
    {
        Body.localScale = new Vector3(ViewDirection * legsOrientation, 1, 1);
    }

    void Update()
    {
        PivotScreenPosition = Camera.main.WorldToScreenPoint(Pivot.position);
        ViewDirection = PivotScreenPosition.x < Input.mousePosition.x ? 1 : -1;

		int alpha1 = (int)KeyCode.Alpha1;

		if (Input.anyKeyDown) {
			for (int i = 0; i < 10; i++) {
				if(Input.GetKey((KeyCode)(alpha1 + i))){
					Debug.Log (i);
					if (GameplayData.Instance.guns.Length > i){

						SelectWeapon(GameplayData.Instance.guns[i]);
					}else{
						SelectWeapon(GameplayData.Instance.tools[i - GameplayData.Instance.guns.Length]);
					}

					break;
				}
			}
		}


	    var p = Grounded;
		Grounded = Physics2D.Linecast(transform.position, grounder.position, groundMask);

        if (!p && Grounded)
        {
            //VFX
            var dust = GameObject.Find("Dust").GetComponent<ParticleSystem>();
            dust.transform.position = transform.position + Vector3.up * 0.05f;
            dust.Play();            
        }

		Animator.SetBool("moving", Input.GetAxis("Horizontal") != 0);
		Animator.SetBool("jump", Input.GetAxis("Jump") != 0);
		Animator.SetBool("grounded", Grounded);
        
    }

    private int viewDirection;
    public int ViewDirection
    {
        get { return viewDirection; }
        set {

            if (viewDirection != value)
            {
                viewDirection = value;
				Body.localScale = new Vector3(value * LegsOrientation, 1, 1);
            }            
        }
    }

	private int legsOrientation = 1;


	public int LegsOrientation
	{
		get { return legsOrientation; }
		set
        {

			if (legsOrientation != value)
            {
				legsOrientation = value;
	            OnMovementDirectionChanged(value);
                transform.localScale = new Vector3(value, 1, 1);
            }
        }
	}


	public void Jump()
	{
		if(Grounded)
			_rigidbody2D.AddForce(NormalJump);
	}

	public void Stop()
	{
		//_rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
	}

	public void HorizontalMove(int d, float normalMoveSpeed)
	{
		_rigidbody2D.velocity = new Vector2(normalMoveSpeed * d, _rigidbody2D.velocity.y);
		LegsOrientation = d;
	}

	public void Drag(float drag)
	{
		_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x * drag, _rigidbody2D.velocity.y);

	}
}
