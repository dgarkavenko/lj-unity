using UnityEngine;
using System.Collections;

public class MStateGunAimContinue : MStateBase {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        var direction = new Vector2(Input.GetAxis("hAim"), Input.GetAxis("vAim") * Dude.Orientation);

        //if (temp.magnitude > 0) direction = temp;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Vector3 localScale;
        if ((Dude.Orientation == -1 && (angle > -60 && angle < 60)) || (Dude.Orientation == 1 && (angle > 120 || angle < -120)))
            localScale = new Vector3(Dude.Orientation, -Dude.Orientation, 1);
        else
            localScale = new Vector3(Dude.Orientation, Dude.Orientation, 1);

        for (int i = 0; i < Dude.Guns; i++)
        {
            var hand = Dude.HandsWithGun[i];
            hand.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            hand.localScale = localScale;
        }
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
