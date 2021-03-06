﻿using UnityEngine;
using System.Collections;

public class MStateJump : MStateBase {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Dude.Jump();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var horInput = Input.GetAxis("Horizontal");

        if (horInput != 0)
            Dude.HorizontalMove(horInput > 0 ? 1 : -1, Dude.NormalMoveSpeed);
        else Dude.Drag(Dude.JumpDrag);


        if (Input.GetKey(KeyCode.Joystick1Button0) && Dude._rigidbody2D.velocity.y > 0)        
            Dude.Lift();
        
        

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
