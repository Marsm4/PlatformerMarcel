using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button speakerButton;         // ������ ��������
    public Image speakerIcon;           // ������ �������� (��������, ��� ������������ ����� ����������/�����������)
    public Sprite speakerOnIcon;        // ������ ����������� ��������
    public Sprite speakerOffIcon;       // ������ ������������ ��������

    private AudioSource audioSource;    // ������ �� ��������� AudioSource
    private bool isMusicOn = true;      // ������ ������ (�������� ��� ���������)

    void Start()
    {
        // ������� ��������� AudioSource �� MainCamera
        audioSource = Camera.main.GetComponent<AudioSource>();

        // ���������, ��� ������ �������� �� ���������
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // ��������� ���������� ��� ������
        speakerButton.onClick.AddListener(ToggleMusic);

        // ��������� ������ � ����������� �� ��������� ������
        UpdateSpeakerIcon();
    }

    // ����������� ��������� ������
    void ToggleMusic()
    {
        isMusicOn = !isMusicOn;

        if (isMusicOn)
        {
            // �������� ������
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // ������������� ������
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }

        // ��������� ������ � ����������� �� ���������
        UpdateSpeakerIcon();
    }

    // �������� ������ ��������
    void UpdateSpeakerIcon()
    {
        if (isMusicOn)
        {
            speakerIcon.sprite = speakerOnIcon;  // ������ ��� ���������� ������
        }
        else
        {
            speakerIcon.sprite = speakerOffIcon; // ������ ��� ����������� ������
        }
    }
}
