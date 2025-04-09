using UnityEngine;

public class Trap : MonoBehaviour
{
    public AudioClip trapSound;
    private bool trapActive = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (trapActive && other.CompareTag("Player"))
        {
            trapActive = false;

            // ����������� ����
            if (trapSound != null)
                AudioSource.PlayClipAtPoint(trapSound, transform.position);

            // �������� ������ ������
            other.GetComponent<PlayerMoving>().Die();

            // ����� 1 ������� ����� ���������� �������
            Invoke("ActivateTrap", 1f);
        }
    }

    void ActivateTrap()
    {
        trapActive = true;
    }
}