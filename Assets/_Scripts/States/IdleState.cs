using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerStateManager stateManager, Animator animator) : base(stateManager, animator)
    {
    }

    public override void Enter()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
    }

    public override void Update()
    {
        if (!stateManager.PlayerCtrl.GroundCheck.IsGrounded)
        {
            stateManager.ChangeState(stateManager.FallState);
        }
    }
}
