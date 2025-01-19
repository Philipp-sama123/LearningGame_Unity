using UnityEngine;

namespace KrazyKatGames
{
    public class ResetFlags : StateMachineBehaviour
    {
        PlayerController player;
        //  OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (player == null) player = animator.gameObject.GetComponent<PlayerController>();
            else
            {
                player.isPerformingAction = false;
            }
        }
    }
}