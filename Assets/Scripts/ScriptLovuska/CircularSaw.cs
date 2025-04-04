using UnityEngine;

public class CircularSaw : MonoBehaviour
{
    [Header("Настройки")]
    public float moveSpeed = 8f;
    public float rotationSpeed = 500f;
    public LayerMask destroyLayers;
    public LayerMask playerLayer;

    [Header("Эффекты")]
    public GameObject hitEffect;
    public AudioClip hitSound;

    void Start()
    {
        // Фиксируем положение по Y
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY |
                           RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Update()
    {
        // Движение строго по горизонтали
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            PlayerMoving player = other.GetComponent<PlayerMoving>();
            if (player != null) player.Die();
            DestroySaw();
        }
        else if (((1 << other.gameObject.layer) & destroyLayers) != 0)
        {
            DestroySaw();
        }
    }

    void DestroySaw()
    {
        if (hitEffect) Instantiate(hitEffect, transform.position, Quaternion.identity);
        if (hitSound) AudioSource.PlayClipAtPoint(hitSound, transform.position);
        Destroy(gameObject);
    }
}