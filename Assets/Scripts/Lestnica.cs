using UnityEngine;

public class Lestnica : MonoBehaviour
{
    [Header("Settings")]
    public float climbSpeed = 3f;
    public float topExitOffset = 0.5f;
    public AudioClip climbingSound;

    private bool playerInLadderZone;
    private PlayerMoving playerMoving;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (playerInLadderZone && playerMoving != null)
        {
            if (Input.GetKey(KeyCode.W))  // Игрок нажимает W
            {
                playerMoving.Climb(climbSpeed);

                // Когда игрок покидает лестницу, остановить подъем
                if (transform.position.y + GetComponent<Collider2D>().bounds.extents.y < playerMoving.transform.position.y - topExitOffset)
                {
                    playerMoving.StopClimbing();
                    playerInLadderZone = false;
                }
            }
            else if (Input.GetKeyUp(KeyCode.W))  // Игрок отпускает W
            {
                playerMoving.StopClimbing();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInLadderZone = true;
            playerMoving = other.GetComponent<PlayerMoving>();
            playerMoving.LadderContact(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerMoving != null)
        {
            playerInLadderZone = false;
            playerMoving.StopClimbing();
            playerMoving.LadderContact(false);
        }
    }
}
