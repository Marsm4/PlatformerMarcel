using UnityEngine;

public class BounceBridge : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceForceMultiplier = 1.5f; // Во сколько раз усиливается прыжок
    public float minBounceVelocity = 5f; // Минимальная скорость отскока
    public float maxBounceVelocity = 15f; // Максимальная скорость отскока

    [Header("Visual Effects")]
    public Animator bridgeAnimator;
    public string bounceAnimationName = "BridgeBounce";
    public ParticleSystem bounceParticles;

    [Header("Audio")]
    public AudioClip bounceSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && bounceSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMoving player = collision.gameObject.GetComponent<PlayerMoving>();
            if (player != null)
            {
                // Усиливаем прыжок
                float currentJumpForce = player.jumpForce * bounceForceMultiplier;
                currentJumpForce = Mathf.Clamp(currentJumpForce, minBounceVelocity, maxBounceVelocity);

                // Применяем усиленный прыжок
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, 0); // Сбрасываем вертикальную скорость
                rb.AddForce(Vector2.up * currentJumpForce, ForceMode2D.Impulse);

                // Визуальные эффекты
                if (bridgeAnimator != null)
                {
                    bridgeAnimator.Play(bounceAnimationName, 0, 0f);
                }

                if (bounceParticles != null)
                {
                    bounceParticles.Play();
                }

                // Звуковой эффект
                if (bounceSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(bounceSound);
                }
            }
        }
    }
}