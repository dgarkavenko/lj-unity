using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    static int PIXELS_IN_METER = 10;

    Camera cam;
    public float scale = 1f;
    public bool update = false;
	public GameObject lumberjack;


    string s;

	void Start () {
        cam = GetComponent<Camera>();
        cam.orthographicSize = Screen.height * 0.5f / scale / PIXELS_IN_METER;

	}


    void OnGUI()
    {

    }
	
	
	/*void Update () {
        if (update) cam.orthographicSize = Screen.height * 0.5f / scale / PIXELS_IN_METER;
	}*/


	void FixedUpdate()
	{
		var target = lumberjack.transform.position;
		var camPos = transform.position;
		
		if (target.x != camPos.x) {
			//camPos.x = Mathf.Lerp(camPos.x, target.x, 0.1f);
			camPos.x = target.x;
		}
		
		transform.position = camPos;
	}
}
