using UnityEngine;

public class BounceBridge : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceForceMultiplier = 1.5f; // �� ������� ��� ����������� ������
    public float minBounceVelocity = 5f; // ����������� �������� �������
    public float maxBounceVelocity = 15f; // ������������ �������� �������

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
                // ��������� ������
                float currentJumpForce = player.jumpForce * bounceForceMultiplier;
                currentJumpForce = Mathf.Clamp(currentJumpForce, minBounceVelocity, maxBounceVelocity);

                // ��������� ��������� ������
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, 0); // ���������� ������������ ��������
                rb.AddForce(Vector2.up * currentJumpForce, ForceMode2D.Impulse);

                // ���������� �������
                if (bridgeAnimator != null)
                {
                    bridgeAnimator.Play(bounceAnimationName, 0, 0f);
                }

                if (bounceParticles != null)
                {
                    bounceParticles.Play();
                }

                // �������� ������
                if (bounceSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(bounceSound);
                }
            }
        }
    }
}