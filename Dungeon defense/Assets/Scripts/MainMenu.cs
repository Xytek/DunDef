using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _start = default;
    [SerializeField] private Button _levelSelect = default;
    [SerializeField] private Button _exit = default;
    // Start is called before the first frame update
    void Start()
    {
        _start.onClick.AddListener(StartClicked);
        _levelSelect.onClick.AddListener(LevelSelectClicked);
        _exit.onClick.AddListener(ExitClicked);
    }

    void StartClicked()
    {
        SceneManager.LoadScene("Level1");
    }

    void LevelSelectClicked()
    {
        SceneManager.LoadScene("Level Select");
    }

    void ExitClicked()
    {
        Application.Quit();
    }
}
