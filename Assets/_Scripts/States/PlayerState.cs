using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateManager stateManager;
    protected Animator animator;

    public PlayerState(PlayerStateManager stateManager, Animator animator)
    {
        this.stateManager = stateManager;
        this.animator = animator;
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void Update()
    {
    }
}
