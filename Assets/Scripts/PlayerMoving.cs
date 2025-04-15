using System.Collections;
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
    private SpriteRenderer spriteRenderer;

    [Header("State")]
    public bool isClimbing;
    public bool canClimb;
    private Collider2D currentLadder;

    [Header("Бессмертие")]
    public bool isImmortal = false;
    public float blinkSpeed = 10f;
    private Color originalColor;

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip[] footstepSounds;
    public float footstepInterval = 0.3f;
    public AudioClip hitByEnemySound;

    private AudioSource audioSource;
    private bool isDead = false;
    private bool hasJumped = false;
    private bool isFootstepPlaying = false;
    private Coroutine jumpCoroutine = null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        rb.freezeRotation = true;
        startPosition = transform.position;
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleClimbing();
        UpdateAnimations();
        HandleFootsteps();
    }

    void HandleFootsteps()
    {
        bool isMovingHorizontally = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        if (isGrounded && isMovingHorizontally && !isClimbing)
        {
            if (!isFootstepPlaying)
            {
                StartFootstepLoop();
            }
        }
        else
        {
            StopFootstepLoop();
        }
    }

    void StartFootstepLoop()
    {
        if (footstepSounds.Length > 0)
        {
            isFootstepPlaying = true;
            audioSource.loop = true;
            audioSource.clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
            audioSource.Play();
        }
    }

    void StopFootstepLoop()
    {
        if (isFootstepPlaying)
        {
            isFootstepPlaying = false;
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
        }
    }

    void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (isClimbing)
        {
            rb.velocity = new Vector2(move * speed * 0.7f, rb.velocity.y);

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
        // Изменено: проверяем как пробел, так и клавишу W
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && (isGrounded || isClimbing))
        {
            hasJumped = true;

            if (isClimbing)
            {
                ExitLadder(new Vector2(0, jumpForce * 0.7f));
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                PlayJumpSound();
            }
        }

        if (isGrounded)
        {
            hasJumped = false;
        }
    }

    void PlayJumpSound()
    {
        if (jumpSound != null && hasJumped)
        {
            if (jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
            }

            jumpCoroutine = StartCoroutine(PlayJumpSoundLimited(jumpSound, 1f));
            hasJumped = false;
        }
    }

    IEnumerator PlayJumpSoundLimited(AudioClip clip, float maxDuration)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.Play();
        yield return new WaitForSeconds(maxDuration);
        if (audioSource.clip == clip)
        {
            audioSource.Stop();
            audioSource.clip = null;
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

    public void Die()
    {
        if (isDead || isImmortal) return;

        isDead = true;
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
            spriteRenderer.color = Color.Lerp(originalColor, Color.cyan, Mathf.PingPong(timer * blinkSpeed, 1));
            timer += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = originalColor;
        isImmortal = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Vrag") && !isImmortal)
        {
            if (hitByEnemySound != null)
            {
                audioSource.PlayOneShot(hitByEnemySound);
            }

            ActivateImmortality(0.1f);
            Die();
        }
    }
}