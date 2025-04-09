/*using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Settings")]
    public int checkpointID;
    public Sprite activeSprite;
    public ParticleSystem activationEffect;

    private SpriteRenderer spriteRenderer;
    private bool isActive;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive && other.CompareTag("Player"))
        {
            isActive = true;
            if (activeSprite != null) spriteRenderer.sprite = activeSprite;
            if (activationEffect != null) activationEffect.Play();

            GameManager.Instance.SetCheckpoint(checkpointID, transform.position);
        }
    }
}*/