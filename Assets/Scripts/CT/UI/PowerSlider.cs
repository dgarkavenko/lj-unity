using CT;
using UnityEngine;
using System.Collections;

public class PowerSlider : MonoBehaviour {



    public Dude Dude;
    public RectTransform rect;
    public RectTransform filler;
    public Vector3 offset;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (Dude.Power <= 0)
        {
            rect.gameObject.SetActive(false);
        }
        else
        {
            rect.gameObject.SetActive(true);
            rect.position = Dude.transform.position + offset;
            filler.localScale = new Vector3(Dude.Power / Dude.MaxPower, 1, 1);
        }

        
	}

    void FixedUpdate()
    {

    }
}
