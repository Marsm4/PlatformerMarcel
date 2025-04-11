using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vragi : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;           // Скорость движения гриба
    public float moveDistance = 5f;        // Максимальное расстояние движения
    public LayerMask groundLayer;          // Для определения, на земле ли объект

    [Header("Damage Settings")]
    public int contactDamage = 1;          // Урон при контакте
    public float bounceForce = 8f;         // Сила отталкивания игрока

    private Vector3 startPosition;         // Начальная позиция гриба
    private Vector3 targetPosition;        // Целевая позиция (куда движется гриб)
    private bool isMovingRight = true;     // Направление движения
    private Rigidbody2D rb;

    void Start()
    {
        startPosition = transform.position;        // Устанавливаем начальную позицию
        targetPosition = startPosition + Vector3.right * moveDistance; // Устанавливаем начальную целевую точку
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // Плавное движение от текущей позиции к целевой с использованием Vector3.MoveTowards
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Если гриб достиг целевой точки, меняем направление
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMovingRight = !isMovingRight;  // Переворачиваем направление
            targetPosition = isMovingRight ? startPosition + Vector3.right * moveDistance : startPosition - Vector3.right * moveDistance;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, прыгнул ли игрок сверху
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y < -0.7f) // Удар сверху
            {
                // Подбрасываем игрока
                collision.gameObject.GetComponent<Rigidbody2D>().velocity =
                    new Vector2(0, bounceForce);

                // Уничтожаем врага
                Destroy(gameObject);
            }
            else // Удар сбоку
            {
                // Наносим урон игроку
                GameManager.Instance.LoseLife();
            }
        }
    }
}
