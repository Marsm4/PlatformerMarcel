using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVrag : MonoBehaviour
{
    [Header("Damage Settings")]
    public int contactDamage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.LoseLife();
        }
    }
}
