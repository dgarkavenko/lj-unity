using UnityEngine;
using System.Collections;

public class LegsController : MonoBehaviour {

    public Animator animator;

    public System.Action<int> directionChangedEvent;

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
                if (directionChangedEvent != null)
                {
                    directionChangedEvent(value);
                }
                transform.localScale = new Vector3(value, 1, 1);
            }
        }
    }

    public float ms;
    public float jump_power;

    private float velocity_x;

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

       
        animator.SetBool("grounded", grounded);

    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(velocity_x, GetComponent<Rigidbody2D>().velocity.y);

    }
}
