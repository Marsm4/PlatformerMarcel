using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [Header("Timing Settings")]
    public float disappearDelay = 7f; // „ерез сколько исчезнет после наступани€
    public float reappearDelay = 5f; // „ерез сколько по€витс€ снова

    [Header("Visual Effects")]
    public ParticleSystem disappearParticles;
    public ParticleSystem reappearParticles;
    public AudioClip disappearSound;
    public AudioClip reappearSound;

    private Collider2D platformCollider;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private bool isActive = true;
    private bool isCounting = false;
    private float timer = 0f;

    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isCounting)
        {
            timer += Time.deltaTime;

            if (isActive && timer >= disappearDelay)
            {
                Disappear();
            }
            else if (!isActive && timer >= reappearDelay)
            {
                Reappear();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Player"))
        {
            StartCounting();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Player"))
        {
            StartCounting();
        }
    }

    void StartCounting()
    {
        if (!isCounting)
        {
            isCounting = true;
            timer = 0f;
        }
    }

    void Disappear()
    {
        isActive = false;
        isCounting = true;
        timer = 0f;

        // ¬изуальное отключение
        platformCollider.enabled = false;
        spriteRenderer.enabled = false;

        // Ёффекты
        if (disappearParticles != null)
        {
            disappearParticles.Play();
        }

        if (disappearSound != null)
        {
            audioSource.PlayOneShot(disappearSound);
        }
    }

    void Reappear()
    {
        isActive = true;
        isCounting = false;

        // ¬изуальное включение
        platformCollider.enabled = true;
        spriteRenderer.enabled = true;

        // Ёффекты
        if (reappearParticles != null)
        {
            reappearParticles.Play();
        }

        if (reappearSound != null)
        {
            audioSource.PlayOneShot(reappearSound);
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = new Color(1, 0.5f, 0, 0.5f);
            Gizmos.DrawCube(transform.position, GetComponent<Collider2D>().bounds.size);
        }
    }
}