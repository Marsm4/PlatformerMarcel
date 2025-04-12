using System.Collections;
using UnityEngine;

public class LightSwitchController : MonoBehaviour
{
    [Header("References")]
    public GameObject darkMask; // ����� ���� (��������, ������ � ����� ����������)
    public AudioClip switchSound; // ���� ��� ���������

    private AudioSource audioSource;
    private bool isActivated = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (darkMask != null)
            darkMask.SetActive(false); // ������� ���� ���������
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            ActivateSwitch();
        }
    }

    void ActivateSwitch()
    {
        isActivated = true;

        if (switchSound != null)
            audioSource.PlayOneShot(switchSound);

        if (darkMask != null)
            darkMask.SetActive(true); // �������� ����

        // (�������������) ��������� ���������� ����:
        // RenderSettings.ambientLight = Color.black;
    }
}
