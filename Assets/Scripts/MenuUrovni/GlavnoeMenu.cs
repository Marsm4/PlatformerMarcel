using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlavnoeMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("UI Buttons")]
    public Button Urovni;
    public Button Skins;

    void Start()
    {
        // ��������� ����������� ������� ��� ������
        Urovni.onClick.AddListener(() => LoadLevel("MainMenu"));
        Skins.onClick.AddListener(() => LoadLevel("Level2"));
        
    }

    // ����� ��� �������� ������ �� �����
    void LoadLevel(string levelName)
    {
        // �������� ����� � ���������, ���������� � �����
        SceneManager.LoadScene(levelName);
    }
}
