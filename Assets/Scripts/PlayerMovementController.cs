using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 5f;           // Movement speed of the player.
    public float jumpForce = 7f;           // Force applied when the player jumps.
    private Rigidbody2D rb;                // Reference to the player's Rigidbody2D component.
    private bool isGrounded;               // Check if the player is grounded.
    public Transform groundCheck;         // Reference to the ground check position.
    public LayerMask groundLayer;         // Layer mask for the ground objects.
    private bool isFacingRight = true;     // Flag to track the direction the player is facing.
    private bool isJumping = false;

    public Animator animator;              // Refernce for the character animations
    private float originalXPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();                  // Get the Rigidbody2D component of the player.
        groundCheck = transform.Find("GroundCheck");       // Find the ground check position.
        groundLayer = LayerMask.GetMask("Ground");          // Set the ground layer (change to your ground layer name).
        rb.freezeRotation = true; // Lock the character's rotation.

        originalXPosition = transform.position.x; // Store the original X position.
                                                  
    }

    private void Update()
    {
        // Check if the player is grounded.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Player movement
        float moveDirection = Input.GetAxis("Horizontal"); // Get the horizontal input (left or right).
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y); // Apply horizontal velocity.


        // Control the animation based on whether the player is jumping or not
        if (isJumping)
        {
            // Play jump animation
            animator.SetBool("isJumping", true); // Assuming you have a "isJumping" parameter in your Animator controller.
        }
        else
        {
            // Play walk animation
            animator.SetBool("isJumping", false);
        }

        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x)); // Set the speed variable to the velocity of the player



        // Flip the character's direction if necessary.
        if (moveDirection > 0 && !isFacingRight)
        {
            FlipCharacter();
        }
        else if (moveDirection < 0 && isFacingRight)
        {
            FlipCharacter();
        }


        // Player jump
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true; // Set isJumping to true when jumping.
        }
        else if (isGrounded)
        {
            isJumping = false; // Set isJumping to false when grounded.
        }
    }

    // Function to flip the character's direction.
    private void FlipCharacter()
    {
        isFacingRight = !isFacingRight;             // Toggle the facing direction flag.

        Vector3 scale = transform.localScale;       // Get the current local scale.
        scale.x *= -1;                             // Invert the X scale to flip horizontally.
        transform.localScale = scale;               // Apply the new local scale to flip the character.

        // Store and reset the character's original X position
        Vector3 position = transform.position;
        position.x = originalXPosition;
        transform.position = position;
    }
}
