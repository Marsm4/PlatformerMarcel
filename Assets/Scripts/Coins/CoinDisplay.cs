using UnityEngine;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private Text coinsText;
    [SerializeField] private string format = "������: {0}";

    void Start()
    {
        UpdateDisplay();
    }

    void OnEnable()
    {
        CoinManager.Instance.AddCoins(0); // ������������� ���� �����
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