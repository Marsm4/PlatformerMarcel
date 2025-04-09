using UnityEngine;

public class Rope : MonoBehaviour
{
    [Header("Rope Swing Settings")]
    public float maxSwingAngle = 80f;
    public float swingSpeed = 6f; // Общая скорость качания
    public Transform bottomPoint;

    [Header("Player Attachment")]
    public KeyCode attachKey = KeyCode.F;
    public float attachRadius = 0.7f;
    public float detachForwardForce = 8f;
    public float detachUpwardForce = 6f;

    [Header("Visuals")]
    public LineRenderer ropeLine;
    public Transform topAnchor;

    // Приватные переменные
    private bool isPlayerAttached = false;
    private Transform playerTransform;
    private Rigidbody2D playerRb;
    private float currentAngle = 0f;
    private float swingDirection = 1f;
    private DistanceJoint2D playerJoint;

    void Start()
    {
        if (ropeLine != null && topAnchor != null)
        {
            ropeLine.positionCount = 2;
            UpdateRopeVisual();
        }
    }

    void Update()
    {
        UpdateRopeVisual();
        HandlePlayerInput();
    }

    void FixedUpdate()
    {
        // Качание каната (одинаковое с игроком и без)
        currentAngle += swingSpeed * swingDirection * Time.fixedDeltaTime;

        // Смена направления
        if (Mathf.Abs(currentAngle) >= maxSwingAngle)
        {
            swingDirection *= -1f;
            currentAngle = Mathf.Clamp(currentAngle, -maxSwingAngle, maxSwingAngle);
        }

        ApplyRotation();
    }

    void ApplyRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    void UpdateRopeVisual()
    {
        if (ropeLine != null && topAnchor != null && bottomPoint != null)
        {
            ropeLine.SetPosition(0, topAnchor.position);
            ropeLine.SetPosition(1, bottomPoint.position);
        }
    }

    void HandlePlayerInput()
    {
        if (isPlayerAttached)
        {
            if (Input.GetKeyUp(attachKey))
            {
                DetachPlayer();
            }
        }
        else
        {
            if (Input.GetKeyDown(attachKey) && IsPlayerNear())
            {
                AttachPlayer();
            }
        }
    }

    bool IsPlayerNear()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                playerRb = player.GetComponent<Rigidbody2D>();
            }
        }

        return playerTransform != null &&
               bottomPoint != null &&
               Vector2.Distance(playerTransform.position, bottomPoint.position) <= attachRadius;
    }

    void AttachPlayer()
    {
        if (playerRb == null || bottomPoint == null) return;

        isPlayerAttached = true;
        playerRb.velocity = Vector2.zero;
        playerRb.gravityScale = 0f;

        // Создаем соединение
        playerJoint = playerRb.gameObject.AddComponent<DistanceJoint2D>();
        playerJoint.connectedBody = bottomPoint.GetComponent<Rigidbody2D>();
        playerJoint.autoConfigureDistance = false;
        playerJoint.distance = 0.2f;
        playerJoint.enableCollision = true;
    }

    void DetachPlayer()
    {
        if (playerRb == null) return;

        isPlayerAttached = false;
        playerRb.gravityScale = 1f;

        // Удаляем соединение
        if (playerJoint != null) Destroy(playerJoint);

        // Рассчитываем направление отталкивания
        Vector2 forwardDirection = new Vector2(Mathf.Sign(currentAngle), 0.5f).normalized;
        Vector2 detachForce = new Vector2(
            forwardDirection.x * detachForwardForce,
            forwardDirection.y * detachUpwardForce
        );

        // Применяем силу
        playerRb.AddForce(detachForce, ForceMode2D.Impulse);
    }

    void OnDrawGizmos()
    {
        if (bottomPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(bottomPoint.position, attachRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(bottomPoint.position, bottomPoint.position +
                new Vector3(Mathf.Sign(currentAngle) * 2f, 1f));
        }
    }
}