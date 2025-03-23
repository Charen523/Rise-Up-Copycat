using UnityEngine;
using UnityEngine.InputSystem;

public class Pointer : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rb;

    private bool isClicked = false;

    #region Unity Life Cycles
    private void Awake()
    {
        spriteRenderer.enabled = false;
    }
    #endregion

    #region Input Method
    public void OnClickEvent(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isClicked = true;
            spriteRenderer.enabled = true;
        }
        else if (context.canceled)
        {
            isClicked = false;
            spriteRenderer.enabled = false;
        }
    }

    public void OnMoveEvent(InputAction.CallbackContext context)
    {
        if (isClicked)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            rb.MovePosition(worldPos);
        }
    }
    #endregion
}