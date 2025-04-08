using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue = 1;
    public AudioClip collectSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Более надежная проверка тега
        if (!other.CompareTag("Player")) return;

        // Проверка GameManager
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        // Добавление очков
        GameManager.Instance.AddScore(scoreValue);

        // Воспроизведение звука
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

        // Уничтожение монеты
        Destroy(gameObject);
    }

    // Визуализация триггера в редакторе
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
    }
}