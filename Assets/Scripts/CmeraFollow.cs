using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmeraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public float swoothSpeed = 0.125f;
    public Vector3 offset;

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = new Vector3(player.position.x, player.position.y, transform.position.z) + offset;
        Vector3 smoothedPostion = Vector3.Lerp(transform.position, desiredPosition, swoothSpeed);

        transform.position = smoothedPostion;
     }
}
