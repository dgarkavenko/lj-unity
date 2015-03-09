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
	public GameObject Hands;

	public Animator Animator;
	   
	private Weapon _currentEquipContainer;
	private Weapon[] _allEquipContainers;

	private Rigidbody2D _rigidbody2D;


	public int JumpPower = 650;
	public int MovementSpeed = 14;

	// --- TEMP ---



	// --- TEMP ---


	void Start () {

		_allEquipContainers = new Weapon[]{new Gun (Hands), new Axe (Hands)};
		_currentEquipContainer = _allEquipContainers [0];

		GunData gd = GameplayData.Instance.guns[0];		
		_currentEquipContainer.SetWeapon(gd);

		_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
		Animator = gameObject.GetComponent<Animator>();

		foreach (var state in Animator.GetBehaviours<LJStateBase>()) state.lj = this;
		
		
	}

	private void SelectWeapon(WeaponData wd){

		Debug.Log ("Switching to: " + wd.alias);

		foreach (var container in _allEquipContainers) {
			if((container.relatedTypes & wd.type) == wd.type){

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
        var pivotScreenPosition = Camera.main.WorldToScreenPoint(Pivot.position);
        ViewDirection = pivotScreenPosition.x < Input.mousePosition.x ? 1 : -1;

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

		if (_currentEquipContainer != null)
			_currentEquipContainer.ManualUpdate (pivotScreenPosition, Pivot.position);
    }


	void FixedUpdate()
	{
		Grounded = Physics2D.Linecast(transform.position, grounder.position, groundMask);
		Animator.SetBool("grounded", Grounded);
		Animator.SetBool("moving", Moving);
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
		_rigidbody2D.AddForce(new Vector2(0, JumpPower));
	}

	public void Move(int d, bool real)
	{
		if (d != 0)
			LegsOrientation = d;

		_rigidbody2D.velocity = new Vector2(MovementSpeed * d, _rigidbody2D.velocity.y);
		
	}

	public void Stop()
	{
		//_rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
	}
}
