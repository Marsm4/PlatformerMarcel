using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public LayerMask spikeLayer; // ��������� ���� ��� �����
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private Vector3 startPosition; // ��������� ������� ��� ��������

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        startPosition = transform.position; // ���������� ��������� �������
    }

    void Update()
    {
        // ��������
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        // ������� ���������
        if (move > 0) transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        if (move < 0) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        // �������� ����
        animator.SetBool("isRunning", move != 0);

        // �������� �����
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.4f, groundLayer);

        // ������
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    // ��������� ������������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���������, ����������� �� � �������� �� ���� �����
        if (((1 << collision.gameObject.layer) & spikeLayer) != 0)
        {
            Die();
        }
    }

    // ��������� ��������� (���� ���� ���������� ��������)
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (((1 << collider.gameObject.layer) & spikeLayer) != 0)
        {
            Die();
        }
    }

    // "������" ������
    private void Die()
    {
        transform.position = startPosition; // ���������� �� ��������� �������
        rb.velocity = Vector2.zero; // ���������� ��������
    }
}