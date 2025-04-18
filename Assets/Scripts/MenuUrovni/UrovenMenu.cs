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
    public Button level4Button;
    public Button level5Button;
    public Button superLevel;
    public Button GlavnoeMenu;
    void Start()
    {
        // ��������� ����������� ������� ��� ������
        level1Button.onClick.AddListener(() => LoadLevel("Level1"));
        level2Button.onClick.AddListener(() => LoadLevel("Level2"));
        level3Button.onClick.AddListener(() => LoadLevel("Level3"));
        level4Button.onClick.AddListener(() => LoadLevel("Level4"));
        level5Button.onClick.AddListener(() => LoadLevel("Level5"));
        GlavnoeMenu.onClick.AddListener(() => LoadLevel("GlavnoeMenu"));
        superLevel.onClick.AddListener(() => LoadLevel("SuperLevel"));
    }

    // ����� ��� �������� ������ �� �����
    void LoadLevel(string levelName)
    {
        // �������� ����� � ���������, ���������� � �����
        SceneManager.LoadScene(levelName);
    }
}
