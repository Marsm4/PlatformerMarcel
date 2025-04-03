using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToporLovuska : MonoBehaviour
{
    [Header("Movement Settings")]
    public float swingAngle = 45f; // ���� ��������� � ��������
    public float swingSpeed = 1f; // �������� ���������

    [Header("Damage Settings")]
    public LayerMask playerLayer; // ���� ������
    public Transform pivotPoint; // ����������� ����� ��������

    private float currentAngle;
    private bool isSwingingRight = true;

    void Start()
    {
        // ���� pivotPoint �� ��������, ���������� ������� �������
        if (pivotPoint == null)
            pivotPoint = transform;

        // ��������� ����
        currentAngle = -swingAngle;
    }

    void Update()
    {
        // ��������� ����� ���������
        if (isSwingingRight)
        {
            currentAngle += swingSpeed * Time.deltaTime;
            if (currentAngle >= swingAngle)
                isSwingingRight = false;
        }
        else
        {
            currentAngle -= swingSpeed * Time.deltaTime;
            if (currentAngle <= -swingAngle)
                isSwingingRight = true;
        }

        // ������� ����� ������ ����� ���������
        transform.RotateAround(pivotPoint.position, Vector3.forward,
                             isSwingingRight ? swingSpeed : -swingSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            PlayerMoving player = other.GetComponent<PlayerMoving>();
            if (player != null) // �������� �� ������, ���� � ������� ��� �������
            {
                player.Die(); // ������ ����� ��������
            }
        }
    }
}
