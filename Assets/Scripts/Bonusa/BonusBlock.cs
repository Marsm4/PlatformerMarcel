using UnityEngine;

public class BonusBlock : MonoBehaviour
{
    [Header("Настройки")]
    public float bounceForce = 8f; // Сила отскока при ударе снизу
    public Vector2 itemSpawnOffset = new Vector2(0, 0.5f); // Смещение для спавна предмета

    [Header("Бонусы")]
    public GameObject[] bonusItems; // Префабы бонусных предметов
    public float immortalityDuration = 30f; // Длительность бессмертия

    [Header("Эффекты")]
    public Animator blockAnimator;
    public ParticleSystem hitParticles;
    public AudioClip hitSound;

    private AudioSource audioSource;
    private bool isBonusUsed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, что игрок ударил блок снизу
        if (!isBonusUsed && collision.gameObject.CompareTag("Player") &&
            collision.contacts[0].normal.y < -0.5f) // Удар снизу
        {
            PlayerHitBlock(collision.gameObject);
        }
    }

    void PlayerHitBlock(GameObject player)
    {
        isBonusUsed = true;

        // 1. Отбрасываем игрока вниз
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -bounceForce);
        }

        // 2. Анимация и эффекты
        if (blockAnimator != null)
        {
            blockAnimator.SetTrigger("Hit");
        }

        if (hitParticles != null)
        {
            hitParticles.Play();
        }

        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // 3. Спавним случайный бонус
        SpawnRandomBonus();
    }

    void SpawnRandomBonus()
    {
        if (bonusItems.Length == 0) return;

        // Выбираем случайный бонус
        int randomIndex = Random.Range(0, bonusItems.Length);
        GameObject bonusPrefab = bonusItems[randomIndex];

        // Создаем бонус
        Vector3 spawnPosition = transform.position + (Vector3)itemSpawnOffset;
        GameObject bonus = Instantiate(bonusPrefab, spawnPosition, Quaternion.identity);

        // Настраиваем бонус
        BonusItem bonusScript = bonus.GetComponent<BonusItem>();
        if (bonusScript != null)
        {
            bonusScript.Setup(immortalityDuration);
        }
    }
}