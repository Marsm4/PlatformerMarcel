using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;  // �������� ��������
    public float moveDistance = 4f; // ����������, ������� ����� ����� ��������� � ���� �������
    public float detectionRange = 3f; // ������ ����������� ������
    public float explosionDelay = 2f; // �������� ����� �������

    [Header("Visual Effects")]
    public ParticleSystem explosionEffect; // ������ ������
    public float blinkInterval = 0.2f; // �������� ��� �������
    public Color blinkColor = Color.red; // ���� �������

    private Vector3 startPosition; // ��������� �������
    private bool movingRight = true; // ���� ����������� ��������
    private SpriteRenderer spriteRenderer; // ��������� ��� ���������
    private Color originalColor; // �������� ���� �������
    private bool isExploding = false; // ���� ��� ������

    void Start()
    {
        startPosition = transform.position; // ��������� ��������� �������
        spriteRenderer = GetComponent<SpriteRenderer>(); // �������� ��������� SpriteRenderer
        originalColor = spriteRenderer.color; // ���������� �������� ���� �������
    }

    void Update()
    {
        if (!isExploding)
        {
            Move(); // ������� �����
            CheckForPlayer(); // ���������, ��������� �� ����� � ������� ������
        }
    }

    // ������� ����� �����-������
    void Move()
    {
        // ��������� ����������� ��������
        float direction = movingRight ? 1 : -1;

        // ������� ����� ������� �� ��� X
        transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);

        // ���� ����� ������ ����������� ����������, ������ �����������
        if (Mathf.Abs(transform.position.x - startPosition.x) >= moveDistance)
        {
            movingRight = !movingRight;
        }
    }

    // ���������, ��������� �� ����� � ������� ������
    void CheckForPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && Vector2.Distance(transform.position, player.transform.position) < detectionRange)
        {
            StartExplosion();
        }
    }

    // ������ ������
    void StartExplosion()
    {
        isExploding = true;
        InvokeRepeating("Blink", 0f, blinkInterval); // �������� �������
        Invoke("Explode", explosionDelay); // ��������� ����� � ���������
    }

    // ������ ������ ����� �������
    void Blink()
    {
        spriteRenderer.color = spriteRenderer.color == originalColor ? blinkColor : originalColor;
    }

    // ��������� �����
    void Explode()
    {
        // ������� ������ ������
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // ���������, ����� �� ����� �� ������
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 3.5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                GameManager.Instance.LoseLife(); // ��������� ����� ������
            }
        }

        // ���������� �����
        Destroy(gameObject);
    }

    // ������ ������ ����������� � ���������
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
