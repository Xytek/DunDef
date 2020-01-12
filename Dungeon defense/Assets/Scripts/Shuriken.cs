using UnityEngine;
using System.Collections;

public class Shuriken : MonoBehaviour
{
    [SerializeField] private GameObject _shurikenEmitter = default; // Spawn point for the shuriken
    [SerializeField] private Trap _shuriken = default;        // Prefab for the shuriken
    [SerializeField] private float _shurikenForce = 1500; // Speed of the shuriken
    private GameObject shurikenHandler;
    void Update()
    {
        if (!GetComponent<PlayerController>().Paused)
            if (Input.GetMouseButtonDown(1))        // On right click
            {
                // Instantiate the shuriken from the emitter
                shurikenHandler = Instantiate(_shuriken.TrapObject, _shurikenEmitter.transform.position, _shurikenEmitter.transform.rotation);
                // Get the rigid body of the shuriken
                Rigidbody rigid = shurikenHandler.GetComponent<Rigidbody>();
                // Push the shuriken forwards in the direction you're facing
                rigid.AddForce(Camera.main.transform.forward * _shurikenForce);
                // Clean up the excessive gameobjects
                Destroy(shurikenHandler, 1.5f);
            }
    }
}