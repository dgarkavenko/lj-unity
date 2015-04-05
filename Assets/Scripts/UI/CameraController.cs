using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    static int PIXELS_IN_METER = 10;

    Camera cam;

    public bool Follow;


    public float Scale = 1f;
	public Transform Target;
    public bool Snap = true;

    private Vector3 _newPosition = new Vector3(0,0,-10f);

    void Start () {
        cam = GetComponent<Camera>();
        cam.orthographicSize = Screen.height * 0.5f / Scale / PIXELS_IN_METER;
	}

	void FixedUpdate()
	{


        if (Target == null || !Follow) return;

        _newPosition.y = transform.position.y;
        _newPosition.x = Snap ? (float)System.Math.Round(Target.position.x, 1, System.MidpointRounding.ToEven) : Target.position.x;
        transform.position = _newPosition;
	}
}


