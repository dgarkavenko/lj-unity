using UnityEngine;
using System.Collections;

public class ZStateBase : StateMachineBehaviour
{

	public static Transform LjTransform;
	public BaseZombie Zombie;

    public Vector2 StateTimeRange;


	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
       
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
           
	}

    public IEnumerator LifetimeCoroutine(){
        var lifetime = Random.RandomRange(StateTimeRange.x, StateTimeRange.y);
        yield return new WaitForSeconds(lifetime);
        OnStateTimeExpired();
    }

   public IEnumerator CooldownCoroutine(float sec, string param)
    {
        Zombie.Animator.SetBool(param, true);
        yield return new WaitForSeconds(sec);
        Zombie.Animator.SetBool(param, false);
    }

    virtual public void OnStateTimeExpired()
    {

    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
