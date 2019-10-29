using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    private GameObject _trap;
    private int _cost;

    public void DestroyTrap(bool sell = false)
    {
        Destroy(_trap);
        _trap = null;
        if (sell)
            GameManager.Instance.playerResources += (int)(_cost * 0.75f);
    }

    public GameObject AddTrap(TrapData trap, RaycastHit hit = default)
    {
        _cost = trap.costToBuild;


        // Ensure they're of the correct type, and decide on their spawn positions
        if (hit.transform.gameObject.tag == "Floor" && trap.type == Type.Floor)
            _trap = Instantiate(trap.objectToSpawn, transform.position, Quaternion.identity, transform);
        if (trap.type == Type.Wall)
            if (hit.transform.gameObject.tag == "xWall")
                _trap = Instantiate(trap.objectToSpawn, new Vector3(hit.point.x, 2.5f, transform.position.z), Quaternion.FromToRotation(Vector3.up, hit.normal), transform);
            else if (hit.transform.gameObject.tag == "zWall")
                _trap = Instantiate(trap.objectToSpawn, new Vector3(transform.position.x, 2.5f, hit.point.z), Quaternion.FromToRotation(Vector3.up, hit.normal), transform);


        return _trap;
    }


    public void FinalizeBuy()
    {
        GameManager.Instance.playerResources -= _cost;
    }


    public bool HasTrap()
    {
        return (_trap != null) ? true : false;  // Return true if a trap exists
    }

}
