using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vragi : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;           // �������� �������� �����
    public float moveDistance = 5f;        // ������������ ���������� ��������
    public LayerMask groundLayer;          // ��� �����������, �� ����� �� ������

    [Header("Damage Settings")]
    public int contactDamage = 1;          // ���� ��� ��������
    public float bounceForce = 8f;         // ���� ������������ ������

    private Vector3 startPosition;         // ��������� ������� �����
    private Vector3 targetPosition;        // ������� ������� (���� �������� ����)
    private bool isMovingRight = true;     // ����������� ��������
    private Rigidbody2D rb;

    void Start()
    {
        startPosition = transform.position;        // ������������� ��������� �������
        targetPosition = startPosition + Vector3.right * moveDistance; // ������������� ��������� ������� �����
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // ������� �������� �� ������� ������� � ������� � �������������� Vector3.MoveTowards
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // ���� ���� ������ ������� �����, ������ �����������
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMovingRight = !isMovingRight;  // �������������� �����������
            targetPosition = isMovingRight ? startPosition + Vector3.right * moveDistance : startPosition - Vector3.right * moveDistance;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ���������, ������� �� ����� ������
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y < -0.7f) // ���� ������
            {
                // ������������ ������
                collision.gameObject.GetComponent<Rigidbody2D>().velocity =
                    new Vector2(0, bounceForce);

                // ���������� �����
                Destroy(gameObject);
            }
            else // ���� �����
            {
                // ������� ���� ������
                GameManager.Instance.LoseLife();
            }
        }
    }
}
