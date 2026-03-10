using UnityEngine;

public class VariableGravity : TMono
{
    [SerializeField] private PlayerCtrl playerCtrl;
    [Header("Variable Gravity Settings")]
    [SerializeField] private float normalGravityScale = 2f;
    public float NormalGravityScale => normalGravityScale;
    [SerializeField] private float fastFallGravityScale = 6f;
    public float FastFallGravityScale => fastFallGravityScale;

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

    private void FixedUpdate()
    {
        this.ApplyVariableGravity();
    }

    private void ApplyVariableGravity()
    {
        // Nếu đang rơi xuống → tăng gravity
        if (playerCtrl.Rb.linearVelocity.y < 0)
        {
            playerCtrl.Rb.gravityScale = fastFallGravityScale;
        }
        // Nếu đang nhảy lên → gravity bình thường
        else
        {
            playerCtrl.Rb.gravityScale = normalGravityScale;
        }
    }
}
