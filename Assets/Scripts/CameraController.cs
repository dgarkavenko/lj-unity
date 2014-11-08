﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


    Camera cam;
    public float scale = 1f;
    public bool update = false;


    string s;

	void Start () {
        cam = GetComponent<Camera>();
        cam.orthographicSize = Screen.height * 0.5f / scale;


        foreach (var item in System.Environment.GetCommandLineArgs())
        {
            s += ":::" + item;
        }
	}


    void OnGUI()
    {

       
        GUILayout.Label(s);
    }
	
	
	void Update () {
        if (update) cam.orthographicSize = Screen.height * 0.5f / scale;	
	}
}