using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerStateManager stateManager, Animator animator) : base(stateManager, animator)
    {
    }

    public override void Enter()
    {
        animator.SetBool("IsJumping", true);
        animator.SetBool("IsReady", false);
    }

    public override void Update()
    {
        // Kiểm tra nếu velocity xuống (đang rơi)
        if (stateManager.PlayerCtrl.Rb.linearVelocity.y < 0)
        {
            stateManager.ChangeState(stateManager.FallState);
        }
    }

    public override void Exit()
    {
        animator.SetBool("IsJumping", false);
    }
}
