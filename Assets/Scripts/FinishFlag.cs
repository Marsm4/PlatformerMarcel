using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class FinishFlag : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip victoryMusic; // Музыка победы
    public float musicDuration = 5f; // Длительность музыки (5 секунд)

    private AudioSource audioSource;
    private bool hasTriggered = false; // Чтобы событие срабатывало только один раз

    void Start()
    {
        // Создаем AudioSource, если его нет
        if (GetComponent<AudioSource>() == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            // Вызываем победу в GameManager
            GameManager.Instance.WinGame();

            // Воспроизводим музыку победы
            if (victoryMusic != null)
            {
                audioSource.clip = victoryMusic;
                audioSource.Play();

                // Останавливаем музыку через заданное время
                StartCoroutine(StopMusicAfterDelay(musicDuration));
            }
        }
    }

    IEnumerator StopMusicAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
    }
}