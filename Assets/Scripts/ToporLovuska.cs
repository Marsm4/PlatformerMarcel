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
    public Animator animator;

    private float currentAngle;
    private bool isSwingingRight = true;
    private AudioSource audioSource;
    private bool trapActive = true;

    void Start()
    {
        if (pivotPoint == null)
            pivotPoint = transform;

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
        if (trapActive && ((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            trapActive = false;

            PlayerMoving player = other.GetComponent<PlayerMoving>();
            if (player != null)
            {
                player.Die();

                if (animator != null)
                    animator.SetTrigger("Hit");

                if (hitSound != null)
                    AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            Invoke("ActivateTrap", 1f);
        }
    }

    void ActivateTrap()
    {
        trapActive = true;
    }
}