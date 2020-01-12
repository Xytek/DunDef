using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.SceneManagement;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject _enemy = default;
    [SerializeField] private GameObject _goal = default;
    [SerializeField] private PlayerController _player = default;
    [SerializeField] private float _spawnRate = default;
    [SerializeField] private int _waveSize = default;
    [SerializeField] private int _level = default;
    [SerializeField] private UIManager _uiManager = default;
    [SerializeField] private ESCMenu _escMenu = default;

    private NavMeshAgent _agent;
    private bool _started;
    private bool _paused;
    private bool _ended;
    private int _spawned = 0;
    private int _killed = 0;
    private int _maxHealth;
    private int _prevStars = 0;
    private List<GameObject> _enemies = new List<GameObject>();
    private List<Transform> _spawns = new List<Transform>();

    private void Start()
    {
        GetComponent<SaveLoad>().LoadFile();
        if (GameManager.Instance.playerStars.Count >= _level)
            _prevStars = GameManager.Instance.playerStars[_level - 1];
        for (int i = 1; i < transform.childCount; i++)
            _spawns.Add(transform.GetChild(i));
    }

    void Update()
    {
        if (_started == false && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SpawnMobs());
            _started = true;
            Debug.Log("Started");
        }
        UpdateEnemyCount();

        if (_ended != true && (_killed == _waveSize || GameManager.Instance.playerHealth == 0))
            GameFinished();

        if (_ended == true && Input.anyKey)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("Level Select");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            if (_paused)
                Resume();
            else
                Pause();

    }

    private void GameFinished()
    {
        Pause(true);
        _ended = true;
        if (GameManager.Instance.playerHealth == 0)
            _uiManager.GameOver();
        else
        {
            int newStars = GiveStars();
            if (_prevStars == 0)
            {
                GameManager.Instance.playerStars.Add(newStars);
                GetComponent<SaveLoad>().SaveFile();
            }
            else if (newStars > _prevStars)
            {
                GameManager.Instance.playerStars[_level - 1] = newStars;
                GetComponent<SaveLoad>().SaveFile();
            }
            _uiManager.GameOver(_maxHealth, GameManager.Instance.playerHealth, newStars);
        }
        Invoke("GameEnded", 2.0f);
    }

    private void Pause(bool gameOver = false)
    {
        if(!gameOver)
            _escMenu.gameObject.SetActive(true);


        _player.Paused = true;
        _paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        foreach (GameObject e in _enemies)
        {
            //e.GetComponent<NavMeshAgent>().speed = 0;
            e.GetComponent<NavMeshAgent>().isStopped = true;
            e.GetComponent<Animator>().SetBool("Idle", true);
        }
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _player.Paused = false;
        _escMenu.gameObject.SetActive(false);

        _paused = false;
        foreach (GameObject e in _enemies)
        {
            //e.GetComponent<NavMeshAgent>().speed = 1;
            e.GetComponent<NavMeshAgent>().isStopped = false;
            e.GetComponent<Animator>().SetBool("Idle", false);
        }
    }

     private void GameLost()
    {
        SceneManager.LoadScene("Level Select");
    }

    private void UpdateEnemyCount()
    {
        _enemies = _enemies.Where(item => item != null).ToList();
        _killed = _spawned - _enemies.Count;
        _uiManager.UpdateMobCount(_waveSize, _killed);
    }

    private IEnumerator SpawnMobs()
    {
        _maxHealth = GameManager.Instance.playerHealth;
        // While there's still enemies left to spawn
        while (_spawned < _waveSize)
        {
            if (!_paused)
            {
                foreach (Transform s in _spawns)
                {
                    if (_spawned == _waveSize)
                        break;
                    GameObject enemy = Instantiate(_enemy, s.position, Quaternion.identity, s.transform);
                    _enemies.Add(enemy);
                    _spawned++;
                    Enemy target = enemy.GetComponent<Enemy>();
                    // Reaching the goal is handled in the individual enemy scripts
                    target.goal = _goal;
                    _agent = enemy.GetComponent<NavMeshAgent>();
                    _agent.SetDestination(_goal.transform.position);
                }

                yield return new WaitForSeconds(_spawnRate);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }



    private int GiveStars()
    {
        int remainingHealth = GameManager.Instance.playerHealth;

        if (remainingHealth == _maxHealth)
            return 3;
        else if (remainingHealth > _maxHealth * 0.75f)
            return 2;
        else
            return 1;
    }
}
