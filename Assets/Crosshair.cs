using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
    
    static private Crosshair _instance;
    private RectTransform _transform;

    public static Crosshair Instance
    {
        get { return _instance; }

    }

    public void Awake()
    {
        _instance = this;
        _transform = GetComponent<RectTransform>();
    }

    public float Size {
        set { _transform.sizeDelta = Vector2.one * Mathf.Max(20, value); }
    }


    // Update is called once per frame
    void Update () {
        _transform.anchoredPosition = Input.mousePosition;
    }
}

