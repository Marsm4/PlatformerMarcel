using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("���������� �������")]
    public Sprite activeSprite; // ������ ��������������� ���������
    private SpriteRenderer spriteRenderer;
    private bool isActive;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            ActivateCheckpoint();
        }
    }

    void ActivateCheckpoint()
    {
        isActive = true;
        // ������ ������
        if (activeSprite != null) spriteRenderer.sprite = activeSprite;

        // ��������� ������� � GameManager
        GameManager.Instance.SetCheckpoint(transform.position);

        // ������� (�����������)
        GetComponent<AudioSource>()?.Play(); // ���� ���������
    }
}