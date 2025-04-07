using UnityEngine;

public class Trap : MonoBehaviour
{
    public AudioClip trapSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ����������� ����
            if (trapSound != null)
                AudioSource.PlayClipAtPoint(trapSound, transform.position);

            // �������� ������ ������
            other.GetComponent<PlayerMoving>().Die();
        }
    }
}