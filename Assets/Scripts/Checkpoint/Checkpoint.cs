using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Визуальные эффекты")]
    public Sprite activeSprite; // Спрайт активированного чекпоинта
    private SpriteRenderer spriteRenderer;
    private bool isActive;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            ActivateCheckpoint();
        }
    }

    void ActivateCheckpoint()
    {
        isActive = true;
        // Меняем спрайт
        if (activeSprite != null) spriteRenderer.sprite = activeSprite;

        // Сохраняем позицию в GameManager
        GameManager.Instance.SetCheckpoint(transform.position);

        // Эффекты (опционально)
        GetComponent<AudioSource>()?.Play(); // Звук активации
    }
}