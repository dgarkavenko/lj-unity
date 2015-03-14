using UnityEngine;
using System.Collections;

public class Dude : MonoBehaviour {

    Rigidbody2D _rigidbody2D;
    public Animator _animator;


    public Transform groundCheck;
    public LayerMask groundMask;
    public float NormalMoveSpeed = 7;
    public float NormalJumpPower;
    public float NormalDrag;

    public float PowerGain = 0.1f;
    
    public bool Grounded;
    public bool DoubleJumpEnabled;


    
	// Use this for initialization
	void Start () {

        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        foreach (var state in _animator.GetBehaviours<MStateBase>()) state.Dude = this;        
	}

    void Update()
    {
        Grounded = Physics2D.Linecast(transform.position, groundCheck.position, groundMask);


        if (Input.GetAxis("Chop") != 0)
            _animator.SetBool("power", true);
        else
            _animator.SetBool("power", false);

        

        _animator.SetBool("walking", Input.GetAxis("Horizontal") != 0);
        _animator.SetBool("grounded", Grounded);
        _animator.SetBool("jump", Input.GetAxis("Jump") != 0);
    }


    internal void HorizontalMove(int d, float power)
    {
        _rigidbody2D.velocity = new Vector2(power * d, _rigidbody2D.velocity.y);
        transform.localScale = new Vector3(d, 1, 1);
    }

    internal void Jump()
    {
        if(Grounded) _rigidbody2D.AddForce(new Vector2(0, NormalJumpPower));
    }

    internal void Drag()
    {
        _rigidbody2D.velocity *= NormalDrag;
    }
}
