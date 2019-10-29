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
        while (_count < _quantity)
        {
            GameObject enemy = Instantiate(_enemy, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
            _agent = enemy.GetComponent<NavMeshAgent>();
            _agent.SetDestination(_goal.transform.position);
            yield return new WaitForSeconds(_spawnRate);
        }
    }
}
