using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue = 1;
    public AudioClip collectSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        // ����� �������� �������� ����
        if (!other.CompareTag("Player")) return;

        // �������� GameManager
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        // ���������� �����
        GameManager.Instance.AddScore(scoreValue);

        // ��������������� �����
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

        // ����������� ������
        Destroy(gameObject);
    }

    // ������������ �������� � ���������
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
    }
}