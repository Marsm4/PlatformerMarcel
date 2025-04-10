using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
   public Vector3 lastCheckpointPosition;
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
    public Text resultText;

    [Header("Звуки")]
    public AudioClip gameOverSound;
    public AudioClip winSound;

    private AudioSource audioSource;

    /*  void Awake()
      {
          if (Instance == null) Instance = this;
          else Destroy(gameObject);
          lastCheckpointPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
          currentLives = maxLives;
          audioSource = GetComponent<AudioSource>();

          // Скрываем панели при старте
          if (gameOverPanel != null) gameOverPanel.SetActive(false);
          if (winPanel != null) winPanel.SetActive(false);
      }
    */
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Инициализируем чекпоинт стартовой позицией игрока
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            lastCheckpointPosition = player.transform.position;
        }

        currentLives = maxLives;
        // ... остальной код
    }
    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
    }
    public void RespawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = lastCheckpointPosition;
        // Дополнительные действия: сброс здоровья, анимация и т.д.
    }
    public void LoseLife()
    {
        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            GameOver();
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
        Time.timeScale = 0f; // Останавливаем игру
        gameOverPanel.SetActive(true);
        resultText.text = $"Вы проиграли!\nСобрано монет: {score}";
        PlaySound(gameOverSound);
    }

    public void WinGame()
    {
        Time.timeScale = 0f; // Останавливаем игру
        winPanel.SetActive(true);
        resultText.text = $"Победа!\nСобрано монет: {score}";
        PlaySound(winSound);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void AddLife(int amount)
    {
        currentLives = Mathf.Min(currentLives + amount, maxLives);
        UpdateLivesUI();

        // Визуальный эффект
        Debug.Log($"Получено {amount} жизней! Всего: {currentLives}");
    }
}