using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 7f;
    public float climbSpeed = 3f;
    public LayerMask groundLayer;
    public float ladderHorizontalExitForce = 2f;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private Vector3 startPosition;

    [Header("State")]
    public bool isClimbing;
    public bool canClimb;
    private Collider2D currentLadder;

    [Header("Бессмертие")]
    public bool isImmortal = false;
    public float blinkSpeed = 10f;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        startPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
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
        float move = Input.GetAxisRaw("Horizontal");

        if (isClimbing)
        {
            // Горизонтальное движение на лестнице
            rb.velocity = new Vector2(move * speed * 0.7f, rb.velocity.y);

            // Проверка выхода с лестницы вбок
            if (move != 0 && !IsTouchingLadder())
            {
                ExitLadder(new Vector2(move * ladderHorizontalExitForce, 0));
            }
        }
        else
        {
            rb.velocity = new Vector2(move * speed, rb.velocity.y);
        }

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
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isClimbing))
        {
            if (isClimbing) ExitLadder(new Vector2(0, jumpForce * 0.7f));
            else rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void HandleClimbing()
    {
        if (!canClimb && isClimbing)
        {
            ExitLadder(Vector2.zero);
        }

        if (canClimb && !isClimbing && Input.GetAxisRaw("Vertical") != 0)
        {
            StartClimbing();
        }

        if (isClimbing)
        {
            float verticalInput = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
        }
    }

    void StartClimbing()
    {
        isClimbing = true;
        rb.gravityScale = 0;
        // Центрируем игрока на лестнице
        if (currentLadder != null)
        {
            Vector2 ladderCenter = currentLadder.bounds.center;
            transform.position = new Vector2(ladderCenter.x, transform.position.y);
        }
    }

    void ExitLadder(Vector2 exitForce)
    {
        isClimbing = false;
        canClimb = false;
        rb.gravityScale = 1;
        rb.velocity += exitForce;
    }

    bool IsTouchingLadder()
    {
        if (currentLadder == null) return false;
        return GetComponent<Collider2D>().IsTouching(currentLadder);
    }

    void UpdateAnimations()
    {
        animator.SetBool("isRunning", Mathf.Abs(rb.velocity.x) > 0.1f && !isClimbing);
        animator.SetBool("isClimbing", isClimbing && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.4f, groundLayer);
    }

    public void SetLadder(Collider2D ladder, bool contact)
    {
        canClimb = contact;
        currentLadder = contact ? ladder : null;
    }
    private bool isDead = false;
    /*public void Die()
    {
        if (isDead) return; // Если уже мертв, выходим

        isDead = true;
        transform.position = startPosition;
        rb.velocity = Vector2.zero;

        if (GameManager.Instance != null)
            GameManager.Instance.LoseLife();

        // Через 1 секунду снова разрешаем смерть
        Invoke("ResetDeath", 1f);
    }*/
    public void Die()
    {
        if (isDead) return;
        if (isImmortal || isDead) return;
        isDead = true;
        // Заменяем startPosition на позицию из GameManager
        transform.position = GameManager.Instance.lastCheckpointPosition;
        rb.velocity = Vector2.zero;

        if (GameManager.Instance != null)
            GameManager.Instance.LoseLife();

        Invoke("ResetDeath", 1f);
    }

    private void ResetDeath()
    {
        isDead = false;
    }
    public void ActivateImmortality(float duration)
    {
        if (!isImmortal)
        {
            StartCoroutine(ImmortalityEffect(duration));
        }
    }

    IEnumerator ImmortalityEffect(float duration)
    {
        isImmortal = true;
        float timer = 0;

        while (timer < duration)
        {
            // Эффект мерцания
            spriteRenderer.color = Color.Lerp(originalColor, Color.cyan, Mathf.PingPong(timer * blinkSpeed, 1));
            timer += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = originalColor;
        isImmortal = false;
    }


}