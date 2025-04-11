using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;  // Скорость движения
    public float moveDistance = 4f; // Расстояние, которое бомба будет двигаться в одну сторону
    public float detectionRange = 3f; // Радиус обнаружения игрока
    public float explosionDelay = 2f; // Задержка перед взрывом

    [Header("Visual Effects")]
    public ParticleSystem explosionEffect; // Эффект взрыва
    public float blinkInterval = 0.2f; // Интервал для мигания
    public Color blinkColor = Color.red; // Цвет мигания

    private Vector3 startPosition; // Начальная позиция
    private bool movingRight = true; // Флаг направления движения
    private SpriteRenderer spriteRenderer; // Компонент для отрисовки
    private Color originalColor; // Исходный цвет спрайта
    private bool isExploding = false; // Флаг для взрыва

    void Start()
    {
        startPosition = transform.position; // Сохраняем стартовую позицию
        spriteRenderer = GetComponent<SpriteRenderer>(); // Получаем компонент SpriteRenderer
        originalColor = spriteRenderer.color; // Запоминаем исходный цвет спрайта
    }

    void Update()
    {
        if (!isExploding)
        {
            Move(); // Двигаем бомбу
            CheckForPlayer(); // Проверяем, находится ли игрок в радиусе взрыва
        }
    }

    // Двигаем бомбу влево-вправо
    void Move()
    {
        // Вычисляем направление движения
        float direction = movingRight ? 1 : -1;

        // Двигаем бомбу вручную по оси X
        transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);

        // Если бомба прошла определённое расстояние, меняем направление
        if (Mathf.Abs(transform.position.x - startPosition.x) >= moveDistance)
        {
            movingRight = !movingRight;
        }
    }

    // Проверяем, находится ли игрок в радиусе взрыва
    void CheckForPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && Vector2.Distance(transform.position, player.transform.position) < detectionRange)
        {
            StartExplosion();
        }
    }

    // Запуск взрыва
    void StartExplosion()
    {
        isExploding = true;
        InvokeRepeating("Blink", 0f, blinkInterval); // Начинаем мигание
        Invoke("Explode", explosionDelay); // Запускаем взрыв с задержкой
    }

    // Мигаем бомбой перед взрывом
    void Blink()
    {
        spriteRenderer.color = spriteRenderer.color == originalColor ? blinkColor : originalColor;
    }

    // Выполняем взрыв
    void Explode()
    {
        // Создаем эффект взрыва
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Проверяем, попал ли взрыв по игроку
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 3.5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                GameManager.Instance.LoseLife(); // Уменьшаем жизнь игрока
            }
        }

        // Уничтожаем бомбу
        Destroy(gameObject);
    }

    // Рисуем радиус обнаружения в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
