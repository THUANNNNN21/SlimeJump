using UnityEngine;

public class ReadyState : PlayerState
{
    public ReadyState(PlayerStateManager stateManager, Animator animator) : base(stateManager, animator)
    {
    }

    public override void Enter()
    {
        animator.SetBool("IsReady", true);
    }

    public override void Update()
    {
        // State này chỉ chuyển khi gọi OnJumpReleased
        // Không cần logic chuyển state ở đây
    }

    public override void Exit()
    {
        animator.SetBool("IsReady", false);
    }
}
