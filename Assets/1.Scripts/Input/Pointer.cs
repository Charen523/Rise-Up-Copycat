using UnityEngine;
using UnityEngine.InputSystem;

public class Pointer : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rb;

    private bool isClicked = false;
    private Vector2 targetPosition;
    private bool shouldMove = false;

    private void Awake()
    {
        spriteRenderer.enabled = false;
    }

    public void OnClickEvent(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isClicked = true;
            spriteRenderer.enabled = true;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            targetPosition = rb.position;
        }
        else if (context.canceled)
        {
            isClicked = false;
            spriteRenderer.enabled = false;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            shouldMove = false;
        }
    }

    public void OnMoveEvent(InputAction.CallbackContext context)
    {
        if (!isClicked) return;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        targetPosition = worldPos;
        shouldMove = true;
    }

    private void FixedUpdate()
    {
        if (isClicked && shouldMove)
        {
            rb.MovePosition(targetPosition);
        }
    }
}