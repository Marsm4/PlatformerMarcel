using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLovushka : MonoBehaviour
{
    public LayerMask playerLayer;
    public AudioClip deathSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            TriggerSpikeTrap(collision.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (((1 << collider.gameObject.layer) & playerLayer) != 0)
        {
            TriggerSpikeTrap(collider.gameObject);
        }
    }

    private void TriggerSpikeTrap(GameObject player)
    {
        PlayerMoving playerScript = player.GetComponent<PlayerMoving>();
        if (playerScript != null)
        {
            // Воспроизведение звука
            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
            }

            playerScript.Die();

            StartCoroutine(playerScript.Respawn());
        }
    }
}