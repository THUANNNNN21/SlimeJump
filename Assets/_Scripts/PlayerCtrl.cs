using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerCtrl : TMono
{
    [SerializeField] private Rigidbody2D rb;
    public Rigidbody2D Rb => rb;
    [SerializeField] private Collider2D col;
    public Collider2D Col => col;
    [SerializeField] Camera mainCamera;
    public Camera MainCamera => mainCamera;
    [SerializeField] private Animator animator;
    public Animator Animator => animator;
    [SerializeField] private GroundCheck groundCheck;
    public GroundCheck GroundCheck => groundCheck;
    [SerializeField] private WallCollision wallCollision;
    public WallCollision WallCollision => wallCollision;
    [SerializeField] private Transform model;
    public Transform Model => model;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRb();
        this.LoadCol();
        this.LoadMainCamera();
        this.LoadAnimator();
        this.LoadGroundCheck();
        this.LoadWallCollision();
        this.LoadModel();
    }

    private void LoadRb()
    {
        if (this.rb == null)
        {
            this.rb = GetComponent<Rigidbody2D>();
        }
        else
        {
            return;
        }
    }

    private void LoadCol()
    {
        if (this.col == null)
        {
            this.col = GetComponent<Collider2D>();
        }
        else
        {
            return;
        }
    }

    private void LoadMainCamera()
    {
        if (this.mainCamera == null)
        {
            this.mainCamera = Camera.main;
        }
        else
        {
            return;
        }
    }

    private void LoadAnimator()
    {
        if (this.animator == null)
        {
            this.animator = GetComponentInChildren<Animator>();
        }
        else
        {
            return;
        }
    }

    private void LoadGroundCheck()
    {
        if (this.groundCheck == null)
        {
            this.groundCheck = GetComponentInChildren<GroundCheck>();
        }
        else
        {
            return;
        }
    }
    private void LoadWallCollision()
    {
        if (this.wallCollision == null)
        {
            this.wallCollision = GetComponentInChildren<WallCollision>();
        }
        else
        {
            return;
        }
    }
    private void LoadModel()
    {
        if (this.model == null)
        {
            this.model = transform.Find("Model");
        }
        else
        {
            return;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.WallCollision != null)
            this.WallCollision.OnWallContact(collision);
    }
}