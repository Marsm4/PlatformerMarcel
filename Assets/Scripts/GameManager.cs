using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Vector3 lastCheckpointPosition;

    [Header("Game State")]
    public bool isPaused = false;

    [Header("Жизни")]
    public int maxLives = 3;
    private int currentLives;
    public Image[] livesImages;

    [Header("Очки")]
    public int score = 0;
    public Text scoreText;

    [Header("Панели")]
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject pausePanel; // Панель паузы
    public Text resultText;

    [Header("Звуки")]
    public AudioClip gameOverSound;
    public AudioClip winSound;
    public AudioClip pauseSound;
    public AudioClip unpauseSound;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
          //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeGame();
    }

    void InitializeGame()
    {
        // Инициализация чекпоинта
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            lastCheckpointPosition = player.transform.position;
        }

        currentLives = maxLives;
        audioSource = GetComponent<AudioSource>();

        // Скрываем все панели при старте
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false); // Гарантируем, что панель паузы скрыта

        // Убедимся, что игра не на паузе при старте
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        // Обработка паузы по клавише Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    #region Pause System
    public void TogglePause()
    {
        // Не позволяем паузить при game over или победе
        if ((gameOverPanel != null && gameOverPanel.activeSelf) ||
            (winPanel != null && winPanel.activeSelf))
            return;

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
        Time.timeScale = 0f; // Останавливаем игру
        if (pausePanel != null)
        {
            pausePanel.SetActive(true); // Показываем панель только при паузе
        }
        if (pauseSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pauseSound);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Возобновляем игру
        if (pausePanel != null)
        {
            pausePanel.SetActive(false); // Скрываем панель
        }
        if (unpauseSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(unpauseSound);
        }
        isPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion

    #region Gameplay Systems
    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
    }

    public void RespawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = lastCheckpointPosition;
            // Дополнительные действия при респавне
        }
    }

    public void LoseLife()
    {
        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            RespawnPlayer();
        }
    }

    void UpdateLivesUI()
    {
        for (int i = 0; i < livesImages.Length; i++)
        {
            livesImages[i].enabled = i < currentLives;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Очки: " + score;
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        resultText.text = $"Вы проиграли!\nСобрано монет: {score}";
        PlaySound(gameOverSound);
    }

    public void WinGame()
    {
        Time.timeScale = 0f;
        winPanel.SetActive(true);
        resultText.text = $"Победа!\nСобрано монет: {score}";
        PlaySound(winSound);
    }

    public void AddLife(int amount)
    {
        currentLives = Mathf.Min(currentLives + amount, maxLives);
        UpdateLivesUI();
    }
    #endregion

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}