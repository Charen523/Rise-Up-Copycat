using UnityEngine;

public class ObjDynamicOnCollision : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField] private MapPattern pattern;

    private readonly string floorTag = "Floor";

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float maxSpeed = 5f;
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        if (transform.position.y == -15)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(floorTag))
        {
            Destroy(gameObject);
            return;
        }

        rb.bodyType = RigidbodyType2D.Dynamic;
        pattern.InvokeChildCollision(collision);
    }
}