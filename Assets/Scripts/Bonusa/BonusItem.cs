using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BonusItem : MonoBehaviour
{
    [Header("Настройки")]
    public float moveSpeed = 3f; // Скорость движения вверх
    public float lifeTime = 5f; // Время жизни, если не подобран

    [Header("Эффекты")]
    public ParticleSystem collectParticles;
    public AudioClip collectSound;

    private float immortalityDuration;
    private bool isCollected = false;

    void Start()
    {
        // Двигаемся вверх при появлении
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, moveSpeed);

        // Уничтожаем, если не подобран
        Destroy(gameObject, lifeTime);
    }

    public void Setup(float immDuration)
    {
        immortalityDuration = immDuration;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            isCollected = true;
            ApplyBonus(other.gameObject);
            PlayCollectEffects();
            Destroy(gameObject);
        }
    }

    void ApplyBonus(GameObject player)
    {
        if (gameObject.CompareTag("Heart")) // +1 жизнь
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.AddLife(1);
            }
        }
        else if (gameObject.CompareTag("Heart"))/*(gameObject.CompareTag("Immortality"))*/ // Бессмертие
        {
            /*  PlayerMoving playerScript = player.GetComponent<PlayerMoving>();
              if (playerScript != null)
              {
                  playerScript.ActivateImmortality(immortalityDuration);
              }*/
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.AddLife(1);
            }
        }
    }

    void PlayCollectEffects()
    {
        if (collectParticles != null)
        {
            Instantiate(collectParticles, transform.position, Quaternion.identity);
        }

        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
    }
}