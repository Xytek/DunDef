using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _goal;
    [SerializeField] private float _spawnRate;
    [SerializeField] private int _quantity;
    private NavMeshAgent _agent;
    private int _count;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnMobs());
    }


    private IEnumerator SpawnMobs()
    {
        // Give the player a few seconds before starting spawns. Can be replaced with a triggered start later
        yield return new WaitForSeconds(5f);
        // While there's still enemies left to spawn
        while (_count < _quantity)
        {
            GameObject enemy = Instantiate(_enemy, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
            Enemy target = enemy.GetComponent<Enemy>();
            // Reaching the goal is handled in the individual enemy scripts
            target.goal = _goal;
            _agent = enemy.GetComponent<NavMeshAgent>();
            _agent.SetDestination(_goal.transform.position);
           
            yield return new WaitForSeconds(_spawnRate);
        }
    }
}
