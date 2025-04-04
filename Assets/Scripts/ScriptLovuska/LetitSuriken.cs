//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LetitSuriken : MonoBehaviour
//{
//    [Header("Settings")]
//    public float damage = 1f;
//    public float destroyDelay = 0.1f;
//    public LayerMask destroyLayers;
//    public LayerMask playerLayer;

//    [Header("Effects")]
//    public GameObject hitEffect;
//    public AudioClip hitSound;

//    private Rigidbody2D rb;
//    private AudioSource audioSource;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        audioSource = GetComponent<AudioSource>();
//        if (audioSource == null)
//            audioSource = gameObject.AddComponent<AudioSource>();
//    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        // Проверка на игрока
//        if (((1 << other.gameObject.layer) & playerLayer) != 0)
//        {
//            PlayerMoving player = other.GetComponent<PlayerMoving>();
//            if (player != null)
//            {
//                player.Die();
//                PlayHitEffects();
//                Destroy(gameObject, destroyDelay);
//            }
//        }
//        // Проверка на разрушаемые объекты (Cloud_Tileset)
//        else if (((1 << other.gameObject.layer) & destroyLayers) != 0)
//        {
//            PlayHitEffects();
//            Destroy(gameObject, destroyDelay);
//        }
//    }

//    void PlayHitEffects()
//    {
//        if (hitEffect != null)
//            Instantiate(hitEffect, transform.position, Quaternion.identity);

//        if (hitSound != null)
//            AudioSource.PlayClipAtPoint(hitSound, transform.position);
//    }
//}