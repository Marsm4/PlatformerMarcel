using System.Collections;
using UnityEngine;

public class Bomba : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;
    public float moveDistance = 4f;
    public float detectionRange = 3f;
    public float explosionDelay = 2f;

    [Header("Visual Effects")]
    public ParticleSystem explosionEffect;
    public float blinkInterval = 0.2f;
    public Color blinkColor = Color.red;

    [Header("Sound Effects")]
    public AudioClip detectionSound;
    public AudioClip explosionSound;
    [Range(0, 1)] public float soundVolume = 0.7f;

    [Header("Fragments")]
    public GameObject shardPrefab;         // Префаб осколка
    public int shardCount = 12;            // Количество осколков
    public float shardForce = 5f;          // Сила разлета

    private Vector3 startPosition;
    private bool movingRight = true;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isExploding = false;
    private AudioSource audioSource;

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!isExploding)
        {
            Move();
            CheckForPlayer();
        }
    }

    void Move()
    {
        float direction = movingRight ? 1 : -1;
        transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);

        if (Mathf.Abs(transform.position.x - startPosition.x) >= moveDistance)
        {
            movingRight = !movingRight;
        }
    }

    void CheckForPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && Vector2.Distance(transform.position, player.transform.position) < detectionRange)
        {
            StartExplosion();
        }
    }

    void StartExplosion()
    {
        if (isExploding) return;

        isExploding = true;

        if (detectionSound != null)
        {
            audioSource.PlayOneShot(detectionSound, soundVolume);
        }

        InvokeRepeating("Blink", 0f, blinkInterval);
        Invoke("Explode", explosionDelay);
    }

    void Blink()
    {
        spriteRenderer.color = spriteRenderer.color == originalColor ? blinkColor : originalColor;
    }

    void Explode()
    {
        if (explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound, soundVolume);
        }

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Урон игроку от центра взрыва
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 3.5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                GameManager.Instance.LoseLife();
            }
        }

        // Генерация осколков
        if (shardPrefab != null)
        {
            SpawnShards();
        }

        Destroy(gameObject, 0.1f);
    }

    void SpawnShards()
    {
        float angleStep = 360f / shardCount;

        for (int i = 0; i < shardCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

            GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);

            Rigidbody2D rb = shard.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(direction * shardForce, ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
