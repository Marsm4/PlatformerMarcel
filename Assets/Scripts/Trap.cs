using UnityEngine;

public class Trap : MonoBehaviour
{
    public AudioClip trapSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Проигрываем звук
            if (trapSound != null)
                AudioSource.PlayClipAtPoint(trapSound, transform.position);

            // Вызываем смерть игрока
            other.GetComponent<PlayerMoving>().Die();
        }
    }
}