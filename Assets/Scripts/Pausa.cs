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
        // ��������� ������
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        // �������� ���� ��� ������
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    void Update()
    {
        // ��������� ������� ������� �����
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
        // ��������� �������
        Time.timeScale = 0f;

        // ��������� ���� �����
        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        // �������������: ���������� ������ UI ��������� ����
    }

    public void ResumeGame()
    {
        // ������������� �������
        Time.timeScale = 1f;

        // ���������� ���� �����
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        isPaused = false;
    }

    public void GoToMainMenu()
    {
        // ������������� ������� ����� ��������� �����
        Time.timeScale = 1f;

        // �������� �������� ����
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        // � ��������� Unity ��� ��������� Play Mode
        // � ��������� ������ - ������� ����������
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // �������������� ������� ��� ������ UI
    public void OnPauseButtonClicked()
    {
        PauseGame();
    }

    public void OnResumeButtonClicked()
    {
        ResumeGame();
    }
}
