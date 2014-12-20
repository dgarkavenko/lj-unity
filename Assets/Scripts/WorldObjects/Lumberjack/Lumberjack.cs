using UnityEngine;
using System.Collections;

public class Lumberjack : MonoBehaviour
{



    public LegsController legs;

    public Transform body;
    public Transform pivot;
	public GameObject hands;
	   
	private Weapon currentEquipContainer;
	private Weapon[] allEquipContainers;

	// --- TEMP ---



	// --- TEMP ---


	void Start () {
        legs.directionChangedEvent += OnMovementDirectionChanged;

		allEquipContainers = new Weapon[]{new Gun (hands), new Axe (hands)};
		currentEquipContainer = allEquipContainers [0];

		GunData gd = GameplayData.Instance.guns[0];		
		currentEquipContainer.SetWeapon(gd);
	}

	private void SelectWeapon(WeaponData wd){

		Debug.Log ("Switching to: " + wd.alias);

		foreach (var container in allEquipContainers) {
			if((container.relatedTypes & wd.type) == wd.type){

				if (currentEquipContainer != container)
				{
					currentEquipContainer.Kill();
					currentEquipContainer = container;
					currentEquipContainer.Init();
				}

				break;
			}
		}

		currentEquipContainer.SetWeapon(wd);		
	}

    private void OnMovementDirectionChanged(int dir)
    {
        body.localScale = new Vector3(ViewDirection * legs.ViewDirection, 1, 1);
    }

    void Update()
    {
        var pivotScreenPosition = Camera.main.WorldToScreenPoint(pivot.position);
        ViewDirection = pivotScreenPosition.x < Input.mousePosition.x ? 1 : -1;

		int alpha1 = (int)KeyCode.Alpha1;

		if (Input.anyKeyDown) {
			for (int i = 0; i < 10; i++) {
				if(Input.GetKey((KeyCode)(alpha1 + i))){
					Debug.Log (i);
					if (GameplayData.Instance.guns.Length > i){

						SelectWeapon(GameplayData.Instance.guns[i]);
					}else{
						SelectWeapon(GameplayData.Instance.tools[i - GameplayData.Instance.guns.Length]);
					}

					break;
				}
			}
		}

		if (currentEquipContainer != null)
			currentEquipContainer.ManualUpdate (pivotScreenPosition, pivot.position);
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
