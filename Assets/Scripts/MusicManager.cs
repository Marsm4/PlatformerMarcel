using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button speakerButton;         // Кнопка динамика
    public Image speakerIcon;           // Иконка динамика (например, для переключения между включением/выключением)
    public Sprite speakerOnIcon;        // Иконка включенного динамика
    public Sprite speakerOffIcon;       // Иконка выключенного динамика

    private AudioSource audioSource;    // Ссылка на компонент AudioSource
    private bool isMusicOn = true;      // Статус музыки (включена или выключена)

    void Start()
    {
        // Находим компонент AudioSource на MainCamera
        audioSource = Camera.main.GetComponent<AudioSource>();

        // Убедитесь, что музыка включена по умолчанию
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Назначаем обработчик для кнопки
        speakerButton.onClick.AddListener(ToggleMusic);

        // Обновляем иконку в зависимости от состояния музыки
        UpdateSpeakerIcon();
    }

    // Переключить состояние музыки
    void ToggleMusic()
    {
        isMusicOn = !isMusicOn;

        if (isMusicOn)
        {
            // Включаем музыку
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Останавливаем музыку
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }

        // Обновляем иконку в зависимости от состояния
        UpdateSpeakerIcon();
    }

    // Обновить иконку динамика
    void UpdateSpeakerIcon()
    {
        if (isMusicOn)
        {
            speakerIcon.sprite = speakerOnIcon;  // Иконка для включенной музыки
        }
        else
        {
            speakerIcon.sprite = speakerOffIcon; // Иконка для выключенной музыки
        }
    }
}
