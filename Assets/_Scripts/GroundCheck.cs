using UnityEngine;

public class GroundCheck : TMono
{
    [SerializeField] private PlayerCtrl playerCtrl;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDelay = 0.2f;
    private float groundCheckTimer = 0f;

    [SerializeField] private bool isGrounded = true;
    public bool IsGrounded => isGrounded;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerCtrl();
    }

    private void LoadPlayerCtrl()
    {
        if (this.playerCtrl == null)
        {
            this.playerCtrl = GetComponentInParent<PlayerCtrl>();
        }
        else
        {
            return;
        }
    }
    private void Update()
    {
        this.UpdateGroundCheck();
    }

    private void UpdateGroundCheck()
    {
        groundCheckTimer -= Time.deltaTime;
        if (groundCheckTimer <= 0)
        {
            this.CheckGround();
            groundCheckTimer = groundCheckDelay;
        }
    }

    private void CheckGround()
    {
        this.isGrounded = this.playerCtrl.Col.IsTouchingLayers(groundLayer);
    }

    public void ResetGroundCheckTimer()
    {
        groundCheckTimer = groundCheckDelay;
    }
}
