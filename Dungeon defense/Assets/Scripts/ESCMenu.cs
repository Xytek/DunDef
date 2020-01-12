using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ESCMenu : MonoBehaviour
{
   
    [SerializeField] private Button _resume = default;
    [SerializeField] private Button _mainmenu = default;
    [SerializeField] private EnemySpawn _enemySpawn = default;
    void Start()
    {
        _resume.onClick.AddListener(ResumeClicked);
        _mainmenu.onClick.AddListener(MainMenuClicked);
    }

    void ResumeClicked()
    {
        _enemySpawn.Resume();
    }

    void MainMenuClicked()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
