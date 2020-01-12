using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                return null;
            return _instance;
        }
    }

    public void Awake()
    {
        _instance = this;
    }

    public int playerHealth  = 1;
    public int playerResources = 300;
    [RangeAttribute(0, 3)] public List<int> playerStars = new List<int>();
}
