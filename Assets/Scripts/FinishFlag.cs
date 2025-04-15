using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class FinishFlag : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip victoryMusic; // ������ ������
    public float musicDuration = 5f; // ������������ ������ (5 ������)

    private AudioSource audioSource;
    private bool hasTriggered = false; // ����� ������� ����������� ������ ���� ���

    void Start()
    {
        // ������� AudioSource, ���� ��� ���
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

            // �������� ������ � GameManager
            GameManager.Instance.WinGame();

            // ������������� ������ ������
            if (victoryMusic != null)
            {
                audioSource.clip = victoryMusic;
                audioSource.Play();

                // ������������� ������ ����� �������� �����
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