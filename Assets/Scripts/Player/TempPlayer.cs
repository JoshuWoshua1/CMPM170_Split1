using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TempPlayer : MonoBehaviour
{
    public ToolBase equippedTool; // Reference to the currently equipped tool
    public Transform toolPivot; // point where the tool will be rotated from. spawns the tool as a child of this transform a few units in front of the player.
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private Vector2 mouseDirection;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        mouseDirection = (mousePosition - playerScreenPosition).normalized;
        if (mouseDirection.sqrMagnitude > 0.01f) // Avoid zero-length direction
        {
            float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
            toolPivot.rotation = Quaternion.Euler(0, 0, angle+90); // Rotate the tool pivot to face the mouse direction, adjusting for sprite orientation
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        rb.linearVelocity = new Vector2(input.x * moveSpeed, rb.linearVelocity.y);
    }

    void OnAttack(InputValue value)
    {
        // make sure to make a maximum interact range once mouse direction is added
        // that way you cant just stand on the ledge and mine very far away blocks
        if (equippedTool != null)
        {
            // Assuming the player is facing right for simplicity; you can add logic to determine facing direction
            Vector3Int targetCoordinate = new Vector3Int(Mathf.RoundToInt(transform.position.x + mouseDirection.x), Mathf.RoundToInt(transform.position.y + mouseDirection.y), 0);
            equippedTool.tryUseTool(targetCoordinate);
        }
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

}
