using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private Vector3 startPosition;

    [Header("State")]
    public bool isClimbing;
    private bool canClimb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        startPosition = transform.position;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleClimbing();
        UpdateAnimations();
    }

    void HandleMovement()
    {
        if (isClimbing) return;

        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        if (move != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(move) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void HandleClimbing()
    {
        if (!canClimb) return;

        if (isClimbing)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 3f;
        }
    }

    void UpdateAnimations()
    {
        animator.SetBool("isRunning", Mathf.Abs(rb.velocity.x) > 0.1f && !isClimbing);
        animator.SetBool("isClimbing", isClimbing);
    }

    public void Climb(float speed)
    {
        if (!canClimb) return;

        isClimbing = true;
        rb.velocity = new Vector2(0, speed);
    }

    public void StopClimbing()
    {
        isClimbing = false;
    }

    public void LadderContact(bool contact)
    {
        canClimb = contact;
        if (!contact) isClimbing = false;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.4f, groundLayer);
    }

    public void Die()
    {
        transform.position = startPosition;
        rb.velocity = Vector2.zero;
    }
}
