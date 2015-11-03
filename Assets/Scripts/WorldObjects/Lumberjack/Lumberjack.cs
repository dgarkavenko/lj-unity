using UnityEngine;
using System.Collections;

public class Lumberjack : Actor
{


    public LayerMask groundMask;
    public bool Grounded = false;
    public bool Moving = false;
    public Transform grounder;

    public Transform Body;
    public Transform Pivot;

    public LumberjackHP Hp = new LumberjackHP();


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

    public float NormalMoveSpeed = 15;
    public float NormalDrag = 1;
    public float JumpDrag = 1;

    public Vector2 LiftPower;
    public Vector2 NormalJump = new Vector2(0, 450);

    public event System.Action<Weapon> OnWeaponSwithced;

    const int Alpha1 = (int)KeyCode.Alpha1;

    private int[] _ammo = new int[] { 100, 20, 30, 40 };


    void Start()
    {

        var r = Hands.GetComponent<SpriteRenderer>();

        var gun = (AllEquipContainers[0] as Gun);
        gun.ReloadDelegate += ReloadAndCheck;

        _currentEquipContainer = AllEquipContainers[0];

        GunData gd = GameplayData.Instance.guns[0];
        SelectWeapon(gd);

        Animator = gameObject.GetComponent<Animator>();

        foreach (var state in Animator.GetBehaviours<LJStateBase>()) state.lj = this;

        _localPivotPosition = transform.InverseTransformPoint(Pivot.transform.position);

        ZStateBase.LjTransform = this.transform;

    }



    public int ReloadAndCheck(GunData.EAmmo type, int max, bool withdraw)
    {
        var count = Mathf.Min(max, _ammo[(int)type]);
        if (withdraw)
        {
            _ammo[(int)type] -= count;
        }
        return count;
    }

    private void SelectWeapon(WeaponData wd)
    {

        Debug.Log("Switching to: " + wd.alias);

        foreach (var container in AllEquipContainers)
        {
            if ((container.RelatedTypes & wd.type) == wd.type)
            {

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
        if (OnWeaponSwithced != null) OnWeaponSwithced(_currentEquipContainer);
    }

    private void OnMovementDirectionChanged(int dir)
    {
        Body.localScale = new Vector3(ViewDirection * legsOrientation, 1, 1);
    }

    private Vector3 _localPivotPosition;

    void Update()
    {
        PivotScreenPosition = Camera.main.WorldToScreenPoint(transform.TransformPoint(_localPivotPosition));
        ViewDirection = PivotScreenPosition.x < Input.mousePosition.x ? 1 : -1;


        if (Input.anyKeyDown)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Input.GetKey((KeyCode)(Alpha1 + i)))
                {

                    if (GameplayData.Instance.guns.Length > i)
                    {
                        SelectWeapon(GameplayData.Instance.guns[i]);
                    }
                    else
                    {
                        SelectWeapon(GameplayData.Instance.tools[i - GameplayData.Instance.guns.Length]);
                    }

                    break;
                }
            }
        }

        var p = Grounded;
        Grounded = Physics2D.Linecast(transform.position, grounder.position, groundMask);

        Animator.SetBool("moving", Input.GetAxis("Horizontal") != 0);
        Animator.SetBool("jump", Input.GetAxis("Jump") != 0);
        Animator.SetBool("grounded", Grounded);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var layer = LayerMask.LayerToName(collision.gameObject.layer);
        if (layer == "Ground")
            VFX.Instance.Effects[1].Play(collision.contacts[0].point + Vector2.up * 0.2f, 0);
    }

    private int viewDirection;
    public int ViewDirection
    {
        get { return viewDirection; }
        set
        {

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
        if (Grounded)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, NormalJump.y * Time.fixedDeltaTime);
        }
    }

    public void Stop()
    {

    }

    public void HorizontalMove(int d, float normalMoveSpeed)
    {
        _rigidbody2D.velocity = new Vector2(normalMoveSpeed * d, _rigidbody2D.velocity.y);
        LegsOrientation = d;
    }



    public class HP
    {

        public float current = 100;
        public float max = 100;

        public bool IsAlive
        {
            get
            {
                return current > 0;
            }
        }

        public float Normal
        {
            get { return current / max; }
        }
    }

    public class LumberjackHP : HP
    {

    }



}
