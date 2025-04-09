using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f; // �������� ��������
    public float leftBound = -5f; // ����� ������� ��������
    public float rightBound = 5f; // ������ ������� ��������
    public bool startMovingRight = true; // �������� �������� ������

    [Header("Effects")]
    public AudioClip movingSound; // ���� �������� (�����������)

    private Vector3 startPosition;
    private bool movingRight;
    private AudioSource audioSource;

    void Start()
    {
        startPosition = transform.position;
        movingRight = startMovingRight;

        // ��������� AudioSource
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
        // �������� ���������
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
        // ������������ ������ �������� � ���������
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
        // ������������ ������ � ���������
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // ����������� ������ �� ���������
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}