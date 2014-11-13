using UnityEngine;
using System.Collections;

public class Trace : MonoBehaviour {


    LineRenderer lr;
    public float fade;
    private float fadeTime;
	// Use this for initialization
	void Start () {
        GameplayData g = GameplayData.Instance;
        lr = GetComponent<LineRenderer>();
	}

    public void Show(Vector3 origin, Vector3 dir)
    {
        gameObject.SetActive(true);
        lr.SetPosition(0, origin);
        lr.SetPosition(1, dir);
        fadeTime = fade;
    }

    public  Color c1 = new Color(1, 1, 1, 0.6f);
    public Color c2 = new Color(1, 1, 1, 0);

    void Update()
    {       
        fadeTime--;
        lr.materials[0].SetColor("_Color", Color.Lerp(c2, c1, fadeTime / fade));
        if (fadeTime <= 0) gameObject.SetActive(false);
        
    }
}
