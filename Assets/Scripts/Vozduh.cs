using UnityEngine;

public class Vozduh : MonoBehaviour
{
    [Header("Настройки")]
    public float liftForce = 10f;
    public float maxHeight = 5f;

    [Header("Эффекты")]
    public ParticleSystem windParticles;
    public AudioClip windSound;

    private AudioSource audioSource;
    private bool isPlayerInside;
    private GameObject player;
    private Rigidbody2D playerRb;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Кэшируем AudioSource
        if (windParticles != null)
            windParticles.Stop();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            player = other.gameObject;
            playerRb = player.GetComponent<Rigidbody2D>();

            if (windParticles != null)
                windParticles.Play();

            // Проверка перед воспроизведением звука
            if (audioSource != null && windSound != null)
                audioSource.PlayOneShot(windSound);
            else
                Debug.LogWarning("Нет AudioSource или звука!");
        }
    }

    void FixedUpdate()
    {
        if (isPlayerInside && playerRb != null)
        {
            float currentHeight = player.transform.position.y - transform.position.y;
            if (currentHeight < maxHeight)
            {
                float forceMultiplier = 1f - (currentHeight / maxHeight);
                playerRb.AddForce(Vector2.up * liftForce * forceMultiplier, ForceMode2D.Force);
            }
        }
    }
}