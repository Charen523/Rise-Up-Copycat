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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        pattern.InvokeChildCollision(collision);

        if (collision.gameObject.CompareTag(floorTag))
        {
            Destroy(gameObject);
        }
    }
}