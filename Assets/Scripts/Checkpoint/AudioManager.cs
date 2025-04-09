/*using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sounds")]
    public AudioClip checkpointSound;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "Checkpoint":
                audioSource.PlayOneShot(checkpointSound);
                break;
        }
    }
}*/