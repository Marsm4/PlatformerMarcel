using System.Collections;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private Vector3 startPosition;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb.freezeRotation = true;
        startPosition = transform.position;
    }

    void Update()
    {
        // Движение
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        // Поворот персонажа
        if (move != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(move) * Mathf.Abs(transform.localScale.x),
                                transform.localScale.y,
                                transform.localScale.z);
        }

        // Анимация бега
        animator.SetBool("isRunning", move != 0);

        // Проверка земли
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.4f, groundLayer);

        // Прыжок
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void Die()
    {
        transform.position = startPosition;
        rb.velocity = Vector2.zero;
    }

    public IEnumerator Respawn()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        transform.position = startPosition;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}