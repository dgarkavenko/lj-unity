using UnityEngine;
using System.Collections;

public class Dummy : MonoBehaviour, IInteractiveObject {

	public enum State
	{
		stand,
		walk,
		melee,
		ranged,
		pursuit
	}

	[System.Flags]
	public enum Conditions
	{
		see_enemy = 1,
		can_stand = 2,
		can_walk = 4,
		can_ranged_attack = 8,
		can_melee_attack = 16
	}

	protected Conditions condition;
	protected State state;
	protected Schedule currentSchedule;

	protected Schedule stand;
	protected Schedule move;


	void Awake(){
		conditionsUpdateTime = conditionsRefreshRate / 60;
	}
	
	public int conditionsRefreshRate = 10;
	protected float conditionsUpdateTime;
	protected float conditionsUpdate = 0;

	void Update(){

		conditionsUpdate += Time.deltaTime;
		
		if (conditionsUpdate >= conditionsUpdateTime) {
			conditionsUpdate -= conditionsUpdateTime;
			GetConditions();
		}

		if (currentSchedule == null)
			SelectNewSchedule();
		
		if (currentSchedule.IsCompleted || currentSchedule.IsInterrupted(condition))
			SelectNewSchedule();

		currentSchedule.ManualUpdate();

	}

	void SelectNewSchedule ()
	{
		switch (state) {
				default:
						break;
		}
	}

	public virtual void Interact(Interaction interaction, IInteractiveObject subject){

	}

	protected virtual void GetConditions(){
		condition = Conditions.can_stand;
		condition |= Conditions.can_walk;
	}

}
