using UnityEngine;
using System.Collections;

public class ZStateWalk : ZStateBase {


    int dir = 1;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Zombie.StartCoroutine(LifetimeCoroutine());
        dir = Random.Range(0, 100) > 50 ? 1 : -1;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Zombie.HorizontalMove(dir, Zombie.NormalMoveSpeed);
    }

    override public void OnStateTimeExpired()
    {
        Zombie.Animator.SetBool("prowl", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
