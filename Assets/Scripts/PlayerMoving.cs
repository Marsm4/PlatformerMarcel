using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public LayerMask spikeLayer; // Добавляем слой для шипов
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private Vector3 startPosition; // Стартовая позиция для респавна

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        startPosition = transform.position; // Запоминаем стартовую позицию
    }

    void Update()
    {
        // Движение
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        // Поворот персонажа
        if (move > 0) transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        if (move < 0) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

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

    // Обработка столкновений
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, столкнулись ли с объектом на слое шипов
        if (((1 << collision.gameObject.layer) & spikeLayer) != 0)
        {
            Die();
        }
    }

    // Обработка триггеров (если шипы используют триггеры)
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (((1 << collider.gameObject.layer) & spikeLayer) != 0)
        {
            Die();
        }
    }

    // "Смерть" игрока
    private void Die()
    {
        transform.position = startPosition; // Возвращаем на стартовую позицию
        rb.velocity = Vector2.zero; // Сбрасываем скорость
    }
}