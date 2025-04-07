using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Жизни")]
    public int maxLives = 3;
    public int currentLives;
    public Image[] livesImages; // Массив изображений сердечек

    [Header("Очки")]
    public int score = 0;
    public Text scoreText;

    [Header("Конец игры")]
    public GameObject gameOverPanel;
    public Text finalScoreText;
    public AudioClip gameOverSound;

    [Header("Победа")]
    public GameObject winPanel;
    public AudioClip winSound;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        currentLives = maxLives;
        audioSource = GetComponent<AudioSource>();
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
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Вы проиграли!\nСобрано монет: " + score;
        PlaySound(gameOverSound);
    }

    public void WinGame()
    {
        Time.timeScale = 0;
        winPanel.SetActive(true);
        PlaySound(winSound);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}