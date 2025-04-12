using System.Collections;
using UnityEngine;

public class LightSwitchController : MonoBehaviour
{
    [Header("References")]
    public GameObject darkMask; // Маска тьмы (например, объект с тёмным материалом)
    public AudioClip switchSound; // Звук при активации

    private AudioSource audioSource;
    private bool isActivated = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (darkMask != null)
            darkMask.SetActive(false); // Сначала тьма выключена
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            ActivateSwitch();
        }
    }

    void ActivateSwitch()
    {
        isActivated = true;

        if (switchSound != null)
            audioSource.PlayOneShot(switchSound);

        if (darkMask != null)
            darkMask.SetActive(true); // Включаем тьму

        // (Необязательно) Отключаем глобальный свет:
        // RenderSettings.ambientLight = Color.black;
    }
}
