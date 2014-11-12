using UnityEngine;
using System.Collections;

public class HandsController : MonoBehaviour {



    public Transform pivot;
    private SpriteRenderer renderer;

	private Weapon currentEquip;

	// Use this for initialization
	void Start () {

		currentEquip = new Gun();
        currentEquip.renderer = GetComponent<SpriteRenderer>();

        GunData gd = GameplayData.Instance.guns[0];		
		currentEquip.SetWeapon(gd);
	}

    public void ManualUpdate(Vector3 pivotScreenPosition, Vector3 pivotPosition)
    {
		//View update
        if (currentEquip != null)
			currentEquip.ManualUpdate (pivotScreenPosition, pivotPosition);

    }   
	
}
