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

            // Проигрываем звук
            if (trapSound != null)
                AudioSource.PlayClipAtPoint(trapSound, transform.position);

            // Вызываем смерть игрока
            other.GetComponent<PlayerMoving>().Die();

            // Через 1 секунду снова активируем ловушку
            Invoke("ActivateTrap", 1f);
        }
    }

    void ActivateTrap()
    {
        trapActive = true;
    }
}