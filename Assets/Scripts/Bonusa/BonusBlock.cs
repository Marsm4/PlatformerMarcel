using UnityEngine;

public class BonusBlock : MonoBehaviour
{
    [Header("���������")]
    public float bounceForce = 8f; // ���� ������� ��� ����� �����
    public Vector2 itemSpawnOffset = new Vector2(0, 0.5f); // �������� ��� ������ ��������

    [Header("������")]
    public GameObject[] bonusItems; // ������� �������� ���������
    public float immortalityDuration = 30f; // ������������ ����������

    [Header("�������")]
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
        // ���������, ��� ����� ������ ���� �����
        if (!isBonusUsed && collision.gameObject.CompareTag("Player") &&
            collision.contacts[0].normal.y < -0.5f) // ���� �����
        {
            PlayerHitBlock(collision.gameObject);
        }
    }

    void PlayerHitBlock(GameObject player)
    {
        isBonusUsed = true;

        // 1. ����������� ������ ����
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -bounceForce);
        }

        // 2. �������� � �������
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

        // 3. ������� ��������� �����
        SpawnRandomBonus();
    }

    void SpawnRandomBonus()
    {
        if (bonusItems.Length == 0) return;

        // �������� ��������� �����
        int randomIndex = Random.Range(0, bonusItems.Length);
        GameObject bonusPrefab = bonusItems[randomIndex];

        // ������� �����
        Vector3 spawnPosition = transform.position + (Vector3)itemSpawnOffset;
        GameObject bonus = Instantiate(bonusPrefab, spawnPosition, Quaternion.identity);

        // ����������� �����
        BonusItem bonusScript = bonus.GetComponent<BonusItem>();
        if (bonusScript != null)
        {
            bonusScript.Setup(immortalityDuration);
        }
    }
}