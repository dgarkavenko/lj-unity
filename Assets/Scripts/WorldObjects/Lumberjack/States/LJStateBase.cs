using UnityEngine;
using System.Collections;

public class LJStateBase : StateMachineBehaviour
{

	public Lumberjack lj;


	public void MoveBehaviour(bool real)
	{
		if (Input.GetKey(KeyCode.A))
		{
			lj.Move(-1, real);
			lj.Moving = true;

		}
		else if (Input.GetKey(KeyCode.D))
		{
			lj.Move(1, real);
			lj.Moving = true;
		}
		else
		{
			lj.Moving = false;
			lj.Stop();
		}
	}	

}
