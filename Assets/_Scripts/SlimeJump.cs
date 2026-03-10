using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeJump : TMono
{
    [SerializeField] private PlayerCtrl playerCtrl;
    [Header("Jump Settings")]
    [SerializeField] private float _forceMultiplier = 5f;
    [SerializeField] private float _maxForce = 10f;
    [SerializeField] private float _maxJumpAngle = 45f; // Góc tối đa từ trục Y (hướng lên)
    [Header("Trajectory Settings")]
    [SerializeField] private int _trajectoryPoints = 50;
    [SerializeField] private float _trajectorySimulationTime = 2f;
    [SerializeField] private Color _trajectoryColor = Color.yellow;
    private Vector2 startPos;
    private Vector2 currentPos;
    [SerializeField] private bool isDragging = false;
    private Vector2[] trajectoryPositions;
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
    void Update()
    {
        this.HandleInput();
    }
    void HandleInput()
    {
        // 📱 MOBILE
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            this.HandleTouch();
        }
        // 🖱 PC / EDITOR
        else if (Mouse.current != null)
        {
            this.HandleMouse();
        }
    }
    void HandleTouch()
    {
        var touch = Touchscreen.current.primaryTouch;

        Vector2 screenPos = touch.position.ReadValue();
        Vector2 worldPos = this.playerCtrl.MainCamera.ScreenToWorldPoint(screenPos);

        if (touch.press.wasPressedThisFrame)
        {
            TryStartDrag(worldPos);
        }

        if (touch.press.isPressed && isDragging)
        {
            currentPos = worldPos;
            FlipModelTowardsDragDirection(startPos - currentPos);
            CalculateTrajectory(startPos - currentPos);
        }

        if (touch.press.wasReleasedThisFrame && isDragging)
        {
            ReleaseJump();
        }
    }

    void HandleMouse()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = this.playerCtrl.MainCamera.ScreenToWorldPoint(screenPos);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryStartDrag(worldPos);
        }

        if (Mouse.current.leftButton.isPressed && isDragging)
        {
            currentPos = worldPos;
            FlipModelTowardsDragDirection(startPos - currentPos);
            CalculateTrajectory(startPos - currentPos);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            ReleaseJump();
        }
    }

    void TryStartDrag(Vector2 worldPos)
    {
        if (!playerCtrl.GroundCheck.IsGrounded)
        {
            return;
        }

        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit != null)
        {
            isDragging = true;
            startPos = worldPos;

            PlayerStateManager stateManager = playerCtrl.GetComponent<PlayerStateManager>();
            if (stateManager != null)
            {
                stateManager.OnTryStartDrag();
            }
        }
    }

    void ReleaseJump()
    {
        Vector2 dragVector = startPos - currentPos;

        dragVector = Vector2.ClampMagnitude(dragVector, _maxForce);

        // Giới hạn hướng jump chỉ hướng lên trong góc cố định
        dragVector = ClampJumpDirection(dragVector);

        playerCtrl.Rb.AddForce(dragVector * _forceMultiplier, ForceMode2D.Impulse);

        isDragging = false;
        startPos = Vector2.zero;
        currentPos = Vector2.zero;

        playerCtrl.GroundCheck.ResetGroundCheckTimer();

        PlayerStateManager stateManager = playerCtrl.GetComponent<PlayerStateManager>();
        if (stateManager != null)
        {
            stateManager.OnJumpReleased();
        }
    }

    Vector2 ClampJumpDirection(Vector2 direction)
    {
        // Tránh hướng xuống - dragVector.y phải >= 0 (hướng lên hoặc ngang)
        if (direction.y < 0)
        {
            direction.y = 0;
        }

        // Giới hạn góc ngang dựa trên _maxJumpAngle
        float maxHorizontal = direction.y * Mathf.Tan(_maxJumpAngle * Mathf.Deg2Rad);
        direction.x = Mathf.Clamp(direction.x, -maxHorizontal, maxHorizontal);

        return direction;
    }

    void FlipModelTowardsDragDirection(Vector2 dragVector)
    {

        // Lật model dựa trên hướng drag (theo trục X)
        Vector3 scale = playerCtrl.transform.localScale;
        if (dragVector.x > 0)
        {
            scale.x = 1f; // Nhìn sang phải
        }
        else if (dragVector.x < 0)
        {
            scale.x = -1f; // Lật gương - nhìn sang trái
        }
        playerCtrl.transform.localScale = scale;
    }

    void CalculateTrajectory(Vector2 dragVector)
    {
        dragVector = Vector2.ClampMagnitude(dragVector, _maxForce);
        dragVector = ClampJumpDirection(dragVector);

        Vector2 jumpForce = dragVector * _forceMultiplier;
        Vector2 initialVelocity = jumpForce / playerCtrl.Rb.mass;

        // Lấy gravity scale từ VariableGravity nếu có
        float normalGravity = 2f;
        float fastFallGravity = 6f;
        var variableGravity = playerCtrl.GetComponent<VariableGravity>();
        if (variableGravity != null)
        {
            normalGravity = variableGravity.NormalGravityScale;
            fastFallGravity = variableGravity.FastFallGravityScale;
        }

        // Lấy damping từ Rigidbody2D
        float linearDrag = playerCtrl.Rb.linearDamping;
        float timeStep = _trajectorySimulationTime / (_trajectoryPoints - 1);

        trajectoryPositions = new Vector2[_trajectoryPoints];
        Vector2 currentPosition = (Vector2)playerCtrl.transform.position;
        Vector2 currentVelocity = initialVelocity;

        for (int i = 0; i < _trajectoryPoints; i++)
        {
            float gravityScale = (currentVelocity.y >= 0) ? normalGravity : fastFallGravity;
            Vector2 gravity = Physics2D.gravity * gravityScale;

            // Euler integration with drag
            trajectoryPositions[i] = currentPosition;
            currentVelocity += gravity * timeStep;
            currentVelocity *= Mathf.Clamp01(1f - linearDrag * timeStep);
            currentPosition += currentVelocity * timeStep;
        }

        // Debug.Log($"dragVector: {dragVector}, jumpForce: {jumpForce}, initialVelocity: {initialVelocity}");
    }

    void OnDrawGizmosSelected()
    {
        if (trajectoryPositions == null || trajectoryPositions.Length == 0)
            return;

        Gizmos.color = _trajectoryColor;

        // Vẽ đường quỹ đạo
        for (int i = 0; i < trajectoryPositions.Length - 1; i++)
        {
            Gizmos.DrawLine(trajectoryPositions[i], trajectoryPositions[i + 1]);
        }

        // Vẽ điểm đích cuối cùng
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(trajectoryPositions[trajectoryPositions.Length - 1], 0.1f);
    }

    void OnDrawGizmos()
    {
        if (!isDragging || trajectoryPositions == null || trajectoryPositions.Length == 0)
            return;

        Gizmos.color = _trajectoryColor;
        for (int i = 0; i < trajectoryPositions.Length - 1; i++)
        {
            Gizmos.DrawLine(trajectoryPositions[i], trajectoryPositions[i + 1]);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(trajectoryPositions[trajectoryPositions.Length - 1], 0.1f);
    }
}