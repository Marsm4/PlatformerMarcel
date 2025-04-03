using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToporLovuska : MonoBehaviour
{
    [Header("Movement Settings")]
    public float swingAngle = 45f; // Угол колебания в градусах
    public float swingSpeed = 1f; // Скорость колебания

    [Header("Damage Settings")]
    public LayerMask playerLayer; // Слой игрока
    public Transform pivotPoint; // Центральная точка вращения

    private float currentAngle;
    private bool isSwingingRight = true;

    void Start()
    {
        // Если pivotPoint не назначен, используем текущую позицию
        if (pivotPoint == null)
            pivotPoint = transform;

        // Начальный угол
        currentAngle = -swingAngle;
    }

    void Update()
    {
        // Вычисляем новое положение
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

        // Вращаем топор вокруг точки крепления
        transform.RotateAround(pivotPoint.position, Vector3.forward,
                             isSwingingRight ? swingSpeed : -swingSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            PlayerMoving player = other.GetComponent<PlayerMoving>();
            if (player != null) // Проверка на случай, если у объекта нет скрипта
            {
                player.Die(); // Теперь метод доступен
            }
        }
    }
}
