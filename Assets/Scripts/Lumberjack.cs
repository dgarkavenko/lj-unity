using UnityEngine;
using System.Collections;

public class Lumberjack : MonoBehaviour {



    public LegsController legs;
    public HandsController hands;
    public Transform body;
    public Transform pivot;
   


	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        var pivotScreenPosition = Camera.main.WorldToScreenPoint(pivot.position);
        ViewDirection = pivotScreenPosition.x < Input.mousePosition.x ? 1 : -1;
        hands.ManualUpdate(pivotScreenPosition);
    }

    private int viewDirection;

    public int ViewDirection
    {
        get { return viewDirection; }
        set {

            if (viewDirection != value)
            {
                viewDirection = value;
                body.localScale = new Vector3(value * legs.ViewDirection, 1, 1);
            }            
        }
    }

   
	

	
}
