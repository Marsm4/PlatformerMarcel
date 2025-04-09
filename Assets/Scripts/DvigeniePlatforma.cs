using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f; // Скорость движения
    public float leftBound = -5f; // Левая граница движения
    public float rightBound = 5f; // Правая граница движения
    public bool startMovingRight = true; // Начинать движение вправо

    [Header("Effects")]
    public AudioClip movingSound; // Звук движения (опционально)

    private Vector3 startPosition;
    private bool movingRight;
    private AudioSource audioSource;

    void Start()
    {
        startPosition = transform.position;
        movingRight = startMovingRight;

        // Настройка AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && movingSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.clip = movingSound;
            audioSource.Play();
        }
    }

    void Update()
    {
        // Движение платформы
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (transform.position.x > startPosition.x + rightBound)
                movingRight = false;
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x < startPosition.x + leftBound)
                movingRight = true;
        }
    }

    void OnDrawGizmos()
    {
        // Визуализация границ движения в редакторе
        if (!Application.isPlaying)
            startPosition = transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPosition + Vector3.right * leftBound,
                        startPosition + Vector3.right * rightBound);
        Gizmos.DrawSphere(startPosition + Vector3.right * leftBound, 0.1f);
        Gizmos.DrawSphere(startPosition + Vector3.right * rightBound, 0.1f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Присоединяем игрока к платформе
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Отсоединяем игрока от платформы
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}