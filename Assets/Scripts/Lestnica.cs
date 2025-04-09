using UnityEngine;

public class Lestnica : MonoBehaviour
{
    [Header("Settings")]
    public float snapDistance = 0.3f;
    public AudioClip climbingSound;

    private PlayerMoving player;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerMoving>();
            player.SetLadder(GetComponent<Collider2D>(), true);

            // јвтоматическое прит€гивание к центру лестницы
            if (Mathf.Abs(other.transform.position.x - transform.position.x) > snapDistance)
            {
                Vector2 newPos = other.transform.position;
                newPos.x = transform.position.x;
                other.transform.position = newPos;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && player != null)
        {
            player.SetLadder(null, false);
            player = null;
        }
    }
}