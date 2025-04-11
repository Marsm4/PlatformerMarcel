using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UrovenMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("UI Buttons")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    void Start()
    {
        // ��������� ����������� ������� ��� ������
        level1Button.onClick.AddListener(() => LoadLevel("Level1"));
        level2Button.onClick.AddListener(() => LoadLevel("Level2"));
        level3Button.onClick.AddListener(() => LoadLevel("Level3"));
    }

    // ����� ��� �������� ������ �� �����
    void LoadLevel(string levelName)
    {
        // �������� ����� � ���������, ���������� � �����
        SceneManager.LoadScene(levelName);
    }
}
