using UnityEngine;

public interface IInteraction {
	
	Interactive.InteractionType InteractionType {get;}
	
}

public class ChopAction : IInteraction{

	public Interactive.InteractionType InteractionType {get{return Interactive.InteractionType.chop;}}
	public int direction;
	public float power;
	public Vector2 point;
}

public class GunShotAction : IInteraction{
	
	public Interactive.InteractionType InteractionType {get{return Interactive.InteractionType.gunshot;}}
	public int direction;
	public Vector2 point;
	public float power;
	public float force;
}
