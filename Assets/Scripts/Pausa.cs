using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pausa : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenu;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button quitButton;

    [Header("Settings")]
    public KeyCode pauseKey = KeyCode.Escape;
    public string mainMenuScene = "MainMenu";

    private bool isPaused = false;

    void Start()
    {
        // Настройка кнопок
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        // Скрываем меню при старте
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    void Update()
    {
        // Обработка нажатия клавиши паузы
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        // Остановка времени
        Time.timeScale = 0f;

        // Включение меню паузы
        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        // Дополнительно: отключение других UI элементов игры
    }

    public void ResumeGame()
    {
        // Возобновление времени
        Time.timeScale = 1f;

        // Выключение меню паузы
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        isPaused = false;
    }

    public void GoToMainMenu()
    {
        // Возобновление времени перед загрузкой сцены
        Time.timeScale = 1f;

        // Загрузка главного меню
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        // В редакторе Unity это остановит Play Mode
        // В собранной версии - закроет приложение
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Дополнительные функции для кнопок UI
    public void OnPauseButtonClicked()
    {
        PauseGame();
    }

    public void OnResumeButtonClicked()
    {
        ResumeGame();
    }
}
