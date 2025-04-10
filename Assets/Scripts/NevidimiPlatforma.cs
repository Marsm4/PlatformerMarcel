using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NevidimiPlatforma : MonoBehaviour
{
    [Header("���������")]
    public float disappearDistance = 2f;
    public float fadeSpeed = 5f;

    // ���� ��������� ���� spriteRenderer
    private SpriteRenderer platformSpriteRenderer;
    private Color originalColor;

    void Start()
    {
        // �������� ��������� � ��������� ������
        platformSpriteRenderer = GetComponent<SpriteRenderer>();

        if (platformSpriteRenderer != null)
        {
            originalColor = platformSpriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer �� ������ �� ������� " + gameObject.name);
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
