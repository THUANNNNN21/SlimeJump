using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerStateManager stateManager, Animator animator) : base(stateManager, animator)
    {
    }

    public override void Enter()
    {
        animator.SetBool("IsFalling", true);
        animator.SetBool("IsJumping", false);
        Time.timeScale = 1.2f;
    }

    public override void Update()
    {
        // Kiểm tra nếu chạm đất
        if (stateManager.PlayerCtrl.GroundCheck.IsGrounded)
        {
            stateManager.ChangeState(stateManager.IdleState);
        }
    }

    public override void Exit()
    {
        animator.SetBool("IsFalling", false);
        Time.timeScale = 1f;
    }
}
