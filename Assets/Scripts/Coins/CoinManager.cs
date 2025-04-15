using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    private int _totalCoins;
    private const string COINS_KEY = "TotalCoins";

    public int TotalCoins => _totalCoins;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCoins();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        _totalCoins += amount;
        PlayerPrefs.SetInt(COINS_KEY, _totalCoins);
        PlayerPrefs.Save();
    }

    private void LoadCoins()
    {
        _totalCoins = PlayerPrefs.GetInt(COINS_KEY, 0);
    }
}