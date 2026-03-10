using System.Reflection.Emit;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float speed = 2f;

    private bool movingToEnd = true;
    private Vector3 _previousPosition;
    public Vector3 Delta { get; private set; }

    void Start()
    {
        if (startPoint == Vector3.zero) startPoint = transform.position;
        _previousPosition = transform.position;
    }

    void Update()
    {
        MovePlatform();
    }

    void LateUpdate()
    {
        Delta = transform.position - _previousPosition;
        _previousPosition = transform.position;
    }

    void MovePlatform()
    {
        Vector3 target = movingToEnd ? endPoint : startPoint;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            movingToEnd = !movingToEnd;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log($"Collision detected with {collision.collider.name}");
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log($"Collision exit detected with {collision.collider.name}");
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
