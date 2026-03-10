using UnityEngine;

public class PlayerStateManager : TMono
{
    [SerializeField] private PlayerCtrl playerCtrl;
    public PlayerCtrl PlayerCtrl => playerCtrl;
    private Animator animator;

    private PlayerState currentState;
    public IdleState IdleState { get; private set; }
    public ReadyState ReadyState { get; private set; }
    public JumpState JumpState { get; private set; }
    public FallState FallState { get; private set; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerCtrl();
        this.animator = playerCtrl.Animator;
    }


    private void LoadPlayerCtrl()
    {
        if (this.playerCtrl == null)
        {
            this.playerCtrl = GetComponent<PlayerCtrl>();
        }
        else
        {
            return;
        }
    }

    private void InitializeStates()
    {
        IdleState = new IdleState(this, animator);
        ReadyState = new ReadyState(this, animator);
        JumpState = new JumpState(this, animator);
        FallState = new FallState(this, animator);

        currentState = IdleState;
        currentState.Enter();
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState == newState)
            return;

        currentState?.Exit();
        currentState = newState;
        // Debug.Log($"Changed to {currentState.GetType().Name}");
        currentState?.Enter();
    }

    public void OnJumpReleased()
    {
        ChangeState(JumpState);
    }

    public void OnTryStartDrag()
    {
        ChangeState(ReadyState);
    }

    protected override void LoadValues()
    {
        base.LoadValues();
        this.InitializeStates();
    }
}
