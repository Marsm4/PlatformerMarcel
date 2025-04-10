using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NevidimiPlatforma : MonoBehaviour
{
    [Header("Настройки")]
    public float disappearDistance = 2f;
    public float fadeSpeed = 5f;

    // Явно объявляем поле spriteRenderer
    private SpriteRenderer platformSpriteRenderer;
    private Color originalColor;

    void Start()
    {
        // Получаем компонент и сохраняем ссылку
        platformSpriteRenderer = GetComponent<SpriteRenderer>();

        if (platformSpriteRenderer != null)
        {
            originalColor = platformSpriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer не найден на объекте " + gameObject.name);
        }
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        bool shouldBeVisible = distance > disappearDistance;

        if (platformSpriteRenderer != null)
        {
            float targetAlpha = shouldBeVisible ? 1f : 0f;
            Color currentColor = platformSpriteRenderer.color;
            currentColor.a = Mathf.Lerp(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
            platformSpriteRenderer.color = currentColor;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, disappearDistance);
    }
}
