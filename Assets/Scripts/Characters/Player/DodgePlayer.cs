using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgePlayer : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("WalkHorizontal", 0);
        animator.SetInteger("WalkVertical", 0);
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement playerScript = animator.GetComponent<PlayerMovement>();
        if (playerScript != null)
        {
            playerScript.isDodging = false;  
        }
    }

}
