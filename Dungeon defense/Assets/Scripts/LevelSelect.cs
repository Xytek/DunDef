using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private Button _level1 = default;
    [SerializeField] private Button _level2 = default;
    [SerializeField] private Button _back = default;
    // Start is called before the first frame update
    private int _levelsDone;
    private List<int> _results = new List<int>();
    private int _count = 0;
    void Start()
    {
        _results = GetComponent<SaveLoad>().LoadFile();
        if (_results == null)
            _count = 0;
        else
            _count = _results.Count;

        if (_count >= 0)
            _level1.onClick.AddListener(Level1Clicked);
        if (_count >= 1)
        {
            _level2.image.color = new Color(255, 255, 255, 255);
            _level2.onClick.AddListener(Level2Clicked);
            _level1.GetComponent<StarsUI>().ViewStars(_results[0]);
        }
        if (_count >= 2)
            _level2.GetComponent<StarsUI>().ViewStars(_results[1]);

        _back.onClick.AddListener(BackClicked);
    }

    void Level1Clicked()
    {
        SceneManager.LoadScene("Level1");
    }

    void Level2Clicked()
    {
        SceneManager.LoadScene("Level2");
    }

    void BackClicked()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
