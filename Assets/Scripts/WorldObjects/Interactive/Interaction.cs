public interface IInteraction {
	
	Interactive.InteractionType InteractionType {get;}
	
}

public class ChopAction : IInteraction{

	public Interactive.InteractionType InteractionType {get{return Interactive.InteractionType.chop;}}
	public int direction;
	public float power;
}
