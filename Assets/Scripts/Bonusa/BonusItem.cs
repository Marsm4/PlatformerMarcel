using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BonusItem : MonoBehaviour
{
    [Header("���������")]
    public float moveSpeed = 3f; // �������� �������� �����
    public float lifeTime = 5f; // ����� �����, ���� �� ��������

    [Header("�������")]
    public ParticleSystem collectParticles;
    public AudioClip collectSound;

    private float immortalityDuration;
    private bool isCollected = false;

    void Start()
    {
        // ��������� ����� ��� ���������
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, moveSpeed);

        // ����������, ���� �� ��������
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
        if (gameObject.CompareTag("Heart")) // +1 �����
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.AddLife(1);
            }
        }
        else if (gameObject.CompareTag("Heart"))/*(gameObject.CompareTag("Immortality"))*/ // ����������
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