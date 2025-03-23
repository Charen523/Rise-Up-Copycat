using UnityEngine;

public class ObjDynamicOnCollision : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField] private MapPattern pattern;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        pattern.InvokeChildCollision(collision);
    }
}
