using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suriken : MonoBehaviour
{
    [Header("Settings")]
    public GameObject sawPrefab;
    public float shootInterval = 5f;
    public float shootForce = 10f;
    public Transform shootPoint;

    [Header("Audio")]
    public AudioClip shootSound;
    private AudioSource audioSource;

    private float timer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        timer = shootInterval; // Первый выстрел сразу
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ShootSaw();
            timer = shootInterval;
        }
    }

    void ShootSaw()
    {
        if (sawPrefab == null) return;

        GameObject saw = Instantiate(sawPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D rb = saw.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce(shootPoint.right * shootForce, ForceMode2D.Impulse);
        }

        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
