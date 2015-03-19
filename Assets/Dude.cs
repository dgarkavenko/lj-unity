using UnityEngine;
using System.Collections;

public class Dude : MonoBehaviour {

	[Header("Debug")]
    public Rigidbody2D _rigidbody2D;
    public Animator _animator;

	[Header("Movement")]

	public Transform groundCheck;
	public LayerMask groundMask;
	public bool Grounded;

	public float NormalMoveSpeed = 7;
	public float NormalDrag;
	public float JumpDrag;
	public Vector2 LiftPower;
	public Vector2 NormalJump = new Vector2(0, 450);
	
	[Header("Axe")]
	public float MaxPower = 1;
	public float PowerGain = 0.1f;
	public float Power;
	public float RayCastDistance = 2;


	[Header("Guns")]
	public float GunInactivityTime = 2;
	
	//Make setters or ignore
	public int Guns = 1;
	public int GunsDrawn = 0;
	public bool GunAim;


	[Header("Children")]
    public ParticleSystem[] JumpParticles;
    public GameObject AxePrefab;
    public Transform[] HandsWithGun;


	private float timeSinceLastAim;
    

    
    
    

    public event System.Action<int> OrientationChanged;
    
	// Use this for initialization
	void Start () {

        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        foreach (var state in _animator.GetBehaviours<MStateBase>()) state.Dude = this;        
	}

    void Update()
    {
        bool pastFrame = Grounded;
        Grounded = Physics2D.Linecast(transform.position, groundCheck.position, groundMask);

        if (pastFrame != Grounded && Grounded)
        {
            foreach (var ps in JumpParticles)
            {
                ps.Emit(10);
            }           
        }

        if (Input.GetAxis("Chop") != 0)
            _animator.SetBool("power", true);
        else
            _animator.SetBool("power", false);


        _animator.SetBool("walking", Input.GetAxis("Horizontal") != 0);
        _animator.SetBool("grounded", Grounded);
        _animator.SetBool("jump", Input.GetAxis("Jump") != 0);
        _animator.SetBool("shitjustgotreal", Input.GetKeyDown(KeyCode.Joystick1Button3));        

        if (Input.GetAxis("hAim") != 0 || Input.GetAxis("vAim") != 0 || Input.GetAxis("Shooting") > 0){
            GunAim = true;
            timeSinceLastAim = 0;
        }

        if (GunAim)
        {
            timeSinceLastAim += Time.deltaTime;
            if (timeSinceLastAim >= GunInactivityTime)
                GunAim = false;
        }


        _animator.SetBool("gun", GunAim);
    }

    

    internal void HorizontalMove(int d, float power)
    {
        _rigidbody2D.velocity = new Vector2(power * d, _rigidbody2D.velocity.y);
        Orientation = d;
    }


    int _orientation = 1;

    public int Orientation
    {
        get { return _orientation; }
        set
        {
            if (value != _orientation)
            {
                _orientation = value;
                transform.localScale = new Vector3(_orientation, 1, 1);
                if (OrientationChanged != null) OrientationChanged(_orientation);
            }
        }
    }

    internal void Jump()
    {
        if (Grounded)        
            _rigidbody2D.AddForce(NormalJump);
        
    }

	RaycastHit2D hit;

	public void Chop()
	{

		hit = Physics2D.Raycast(transform.position + Vector3.up, new Vector2(Orientation, 0), RayCastDistance, LayerMask.GetMask("Zombies", "Trees"));


		if (hit.collider != null)
		{
			var interactor = hit.collider.gameObject.GetComponent<Interactive>();

			if (interactor != null)
				interactor.Interact(new ChopAction { power = Power, direction = Orientation, point = hit.point });
		}
	}

    internal void Drag(float drag)
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x * drag, _rigidbody2D.velocity.y);
    }

    internal void Lift()
    {
        _rigidbody2D.AddRelativeForce(LiftPower, ForceMode2D.Impulse);
    }


    internal void DropAxe()
    {
        timeSinceLastAim = 0;
        GunAim = true;

        var axe = GameObject.Instantiate(AxePrefab);
        axe.transform.position = transform.position + Vector3.right * Orientation;
        axe.transform.rotation = Quaternion.identity;
        axe.transform.localRotation = transform.localRotation;
       
    }

    internal void Shot(int hands)
    {
        timeSinceLastAim = 0;
        for (int i = 0; i < hands; i++)
        {
            HandsWithGun[i].gameObject.GetComponentInChildren<ParticleSystem>().Emit(10);
        }
    }

}
