using UnityEngine;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private Text coinsText;
    [SerializeField] private string format = "Монеты: {0}";

    void Start()
    {
        UpdateDisplay();
    }

    void OnEnable()
    {
        CoinManager.Instance.AddCoins(0); // Инициализация если нужно
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (coinsText != null && CoinManager.Instance != null)
        {
            coinsText.text = string.Format(format, CoinManager.Instance.TotalCoins);
        }
    }
}