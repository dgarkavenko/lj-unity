using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    static int PIXELS_IN_METER = 10;
    static float HEIGHT;
    Camera cam;

    public bool Follow;
    public Vector2 Bounds;
    public float Scale = 1f;
	public Transform Target;
    public bool Snap = true;

    private Vector3 _newPosition = new Vector3(0,0,-10f);
    private float _width;


    public ParallaxLayer[] Parallax;

    void Start () {
        cam = GetComponent<Camera>();
        cam.orthographicSize = Screen.height * 0.5f / Scale / PIXELS_IN_METER;
        _width = Screen.width * 0.5f / PIXELS_IN_METER / Scale;

        _newPosition.y = Screen.height * 0.5f / PIXELS_IN_METER / Scale - 3.2f; //3.2f stands for ground height
     
    }

	void LateUpdate()
	{
        if (Target == null || !Follow) return;

        
        _newPosition.x = Snap ? (float)System.Math.Round(Target.position.x, 1, System.MidpointRounding.ToEven) : Target.position.x;
        _newPosition.x = Mathf.Clamp(_newPosition.x, Bounds.x + _width, Bounds.y - _width);

        transform.position = _newPosition;

        if (Parallax == null || Parallax.Length < 1) return;

        var cameraCenter = new Vector2(transform.position.x, transform.position.y);

        foreach (var layer in Parallax)
        {
            var layerShift = cameraCenter / (layer.Distance + 1);
            layer.transform.position = new Vector3(cameraCenter.x - layerShift.x, layer.transform.position.y, layer.transform.position.z);
        }

	}

   
}


