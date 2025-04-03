using UnityEngine;

public class AxeTrap : MonoBehaviour
{
    [Header("Movement Settings")]
    public float swingAngle = 45f;
    public float swingSpeed = 1f;

    [Header("Damage Settings")]
    public LayerMask playerLayer;
    public Transform pivotPoint;

    [Header("Effects")]
    public AudioClip hitSound;
    public Animator animator; // Ссылка на аниматор

    private float currentAngle;
    private bool isSwingingRight = true;
    private AudioSource audioSource;

    void Start()
    {
        if (pivotPoint == null)
            pivotPoint = transform;

        // Получаем или создаем AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        currentAngle = -swingAngle;
    }

    void Update()
    {
        if (isSwingingRight)
        {
            currentAngle += swingSpeed * Time.deltaTime;
            if (currentAngle >= swingAngle)
                isSwingingRight = false;
        }
        else
        {
            currentAngle -= swingSpeed * Time.deltaTime;
            if (currentAngle <= -swingAngle)
                isSwingingRight = true;
        }

        transform.RotateAround(pivotPoint.position, Vector3.forward,
                             isSwingingRight ? swingSpeed : -swingSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            PlayerMoving player = other.GetComponent<PlayerMoving>();
            if (player != null)
            {
                player.Die();

                // Проверяем наличие аниматора перед использованием
                if (animator != null)
                {
                    animator.SetTrigger("Hit");
                }

                // Воспроизводим звук
                if (hitSound != null)
                {
                    AudioSource.PlayClipAtPoint(hitSound, transform.position);
                }
            }
        }
    }
}