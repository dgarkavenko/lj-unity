using UnityEngine;
using CT;
using System.Collections;

public class MStateGunAim : MStateBase {


    public bool skipEnter;
    public bool skipExit;


    private Transform[] HandsWithGun = new Transform[]{};
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (skipEnter) return;

		var handsAffected = Dude.CurrentWeapon == Dude.WeaponType.DoubleBeretta ? 2 : 1;
		HandsWithGun = new Transform[handsAffected];

		for (int i = 0; i < handsAffected; i++)
        {
            var  hand = Dude.HandsWithGun[i];
            hand.gameObject.SetActive(true);
            HandsWithGun[i] = hand;
        }       
	}


    bool down = false;

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        var direction = new Vector2(Input.GetAxis("hAim"), Input.GetAxis("vAim"));

        if (direction.magnitude == 0) direction = new Vector2(Dude.Orientation, 0);

		float angle;
        bool flip;

		if (Dude.Orientation == 1){
            angle = Mathf.Atan2(direction.y, direction.x);
            flip = (angle > 2 || angle < -2);
        }
        else
        {
            angle = Mathf.Atan2(direction.y, -direction.x) + Mathf.PI;
            flip = !(angle > 5 || angle < 1);
        }

		foreach (var hand in HandsWithGun)
        {
			hand.localScale = new Vector3(Dude.Orientation, flip ? -1 : 1, 1);
			hand.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        }

        var prevState = down;
        down = Input.GetAxis("Shooting") > 0f;
               
        
        if (down && !prevState)
		{
			Dude.Shot(direction);
		}   

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (skipExit) return;

		foreach (var hand in HandsWithGun)		
			hand.gameObject.SetActive(false);
		
      
        Dude.GunAim = false;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
