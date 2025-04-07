using UnityEngine;

public class FinishFlag : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.WinGame();
        }
    }
}