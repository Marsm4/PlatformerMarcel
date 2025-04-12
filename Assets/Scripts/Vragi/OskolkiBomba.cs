using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OskolkiBomba : MonoBehaviour
{
    public float lifeTime = 2f; // ����� ������� ������ ������������

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.LoseLife(); // ���� ������
            Destroy(gameObject);
        }
    }
}