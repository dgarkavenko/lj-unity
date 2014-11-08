using UnityEngine;
using System.Collections;

public class LegsController : MonoBehaviour {

    public Animator animator;

    public enum Direction
    {
        left = -1,
        right = 1
    }

    private int viewDirection = (int)Direction.right;

    public int ViewDirection
    {
        get { return viewDirection; }
        set
        {

            if (viewDirection != value)
            {
                viewDirection = value;
                transform.localScale = new Vector3(value, 1, 1);
            }
        }
    }

    public float ms;
    public float jump_power;

    private float velocity_x;

    public float groundedDistance = 0.1f;
    public LayerMask groundMask;
    public bool grounded = false;
    public Transform grounder;

	// Use this for initialization
	void Start () {
	
	}

    

    // Update is called once per frame

    void Update()
    {

        velocity_x = 0;

        if (Input.GetKey(KeyCode.A))
        {
            velocity_x = -ms;
            ViewDirection = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velocity_x = ms;
            ViewDirection = 1;
        }

        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            rigidbody2D.AddForce(new Vector2(0, jump_power));
            grounded = false;
        }

        animator.SetBool("grounded", grounded);
        animator.SetInteger("direction", velocity_x > 0 ? 1 : (velocity_x < 0 ? -1 : 0));

    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(grounder.position, groundedDistance, groundMask);
        rigidbody2D.velocity = new Vector2(velocity_x, rigidbody2D.velocity.y);

    }
}
