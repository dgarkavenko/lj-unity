using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {


    protected Rigidbody2D _rigidbody2D;
	// Use this for initialization
	public virtual void Awake () {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

	}

    public void Drag(float drag)
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x * drag, _rigidbody2D.velocity.y);
    }
	
}
