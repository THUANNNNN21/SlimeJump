using UnityEngine;

public class WallCollision : TMono
{
    [SerializeField] private PlayerCtrl playerCtrl;
    [Header("Wall Collision Settings")]
    [SerializeField] private float pushbackForce = 5f;
    [SerializeField] private float velocityMultiplier = 0.5f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float minVelocityForBounce = 0.5f;
    public float veloccity;

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
    void FixedUpdate()
    {
        veloccity = playerCtrl.Rb.linearVelocity.magnitude;
    }
    public void OnWallContact(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & wallLayer) == 0)
        {
            // Debug.Log("Collided with non-wall object, ignoring.");
            return;
        }

        float collisionVelocity = collision.relativeVelocity.magnitude;

        if (collisionVelocity < minVelocityForBounce)
        {
            // Debug.Log("Collision velocity too low, no pushback applied.");
            return;
        }

        Vector2 normal = collision.relativeVelocity.normalized;

        float force = pushbackForce + (collisionVelocity * velocityMultiplier);

        playerCtrl.Rb.linearVelocity = Vector2.zero;
        playerCtrl.Rb.AddForce(normal * force, ForceMode2D.Impulse);

        Debug.Log($"Wall collision detected! Velocity: {collisionVelocity}, Force: {force}");
    }
}
