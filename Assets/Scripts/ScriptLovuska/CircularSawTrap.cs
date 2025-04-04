//using UnityEngine;

using UnityEngine;

public class CircularSawTrap : MonoBehaviour
{
    public GameObject sawPrefab;
    public float spawnInterval = 5f;
    public float spawnOffset = 1f;
    public AudioClip spawnSound;

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnSaw();
            timer = spawnInterval;
        }
    }
    void SpawnSaw()
    {
        if (!sawPrefab) return;

        // Точное позиционирование по Y
        Vector3 spawnPos = new Vector3(
            transform.position.x + spawnOffset,
            transform.position.y,  // Сохраняем ту же высоту, что и у спавнера
            transform.position.z
        );

        Instantiate(sawPrefab, spawnPos, Quaternion.identity);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}