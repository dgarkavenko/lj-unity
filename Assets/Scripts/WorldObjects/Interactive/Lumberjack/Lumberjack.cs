using UnityEngine;
using System.Collections;

public class Lumberjack : MonoBehaviour, IInteractiveObject
{



    public LegsController legs;
    public HandsController hands;
    public Transform body;
    public Transform pivot;
   


	// Use this for initialization
	void Start () {
        legs.directionChangedEvent += OnMovementDirectionChanged;
	}

    private void OnMovementDirectionChanged(int dir)
    {
        body.localScale = new Vector3(ViewDirection * legs.ViewDirection, 1, 1);
    }

    void Update()
    {
        var pivotScreenPosition = Camera.main.WorldToScreenPoint(pivot.position);
        ViewDirection = pivotScreenPosition.x < Input.mousePosition.x ? 1 : -1;
		hands.ManualUpdate(pivotScreenPosition, pivot.position);
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

    public void Interact(Interaction interaction, IInteractiveObject subject)
    {
        switch (interaction.type)
        {
            case Interaction.InteractionType.gunshot:
            case Interaction.InteractionType.chop:
            case Interaction.InteractionType.chainsaw:
                break;
            case Interaction.InteractionType.caboom:
                break;
            case Interaction.InteractionType.treehit:
                break;
            case Interaction.InteractionType.bite:
                break;
            default:
                break;
        }
    }
}
