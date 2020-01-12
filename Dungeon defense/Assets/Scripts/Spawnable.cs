using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    private GameObject _trapGO;
    public Trap Trap;
    private GameObject _preview;
    private int _cost;

    public void DestroyTrap(bool sell = false)
    {
        Destroy(_trapGO);
        Destroy(_preview);
        _trapGO = null;
        _preview = null;
        if (sell)
            GameManager.Instance.playerResources += (int)(_cost * 0.75f);
    }

    public GameObject AddTrap(Trap trap, RaycastHit hit = default)
    {
        _cost = trap.Price;
        Trap = trap;

        // Ensure they're of the correct type, and decide on their spawn positions
        if (hit.transform.gameObject.tag == "Floor" && trap.Type == TrapType.Floor)
            _trapGO = Instantiate(trap.TrapObject, transform.position, Quaternion.identity, transform);
        if (trap.Type == TrapType.Wall)
            if (hit.transform.gameObject.tag == "xWall")
                _trapGO = Instantiate(trap.TrapObject, new Vector3(hit.point.x, 2.5f, transform.position.z), Quaternion.FromToRotation(Vector3.up, hit.normal), transform);
            else if (hit.transform.gameObject.tag == "zWall")
                _trapGO = Instantiate(trap.TrapObject, new Vector3(transform.position.x, 2.5f, hit.point.z), Quaternion.FromToRotation(Vector3.up, hit.normal), transform);

        if (_preview != null)
        {
            Destroy(_preview);
            _preview = null;
        } 
        return _trapGO;
    }

    public GameObject PreviewTrap(Trap trap, RaycastHit hit = default)
    {
        // Ensure they're of the correct type, and decide on their spawn positions
        if (hit.transform.gameObject.tag == "Floor" && trap.Type == TrapType.Floor)
            _preview = Instantiate(trap.PreviewObject, transform.position, Quaternion.identity, transform);
        if (trap.Type == TrapType.Wall)
            if (hit.transform.gameObject.tag == "xWall")
                _preview = Instantiate(trap.PreviewObject, new Vector3(hit.point.x, 2.5f, transform.position.z), Quaternion.FromToRotation(Vector3.up, hit.normal), transform);
            else if (hit.transform.gameObject.tag == "zWall")
                _preview = Instantiate(trap.PreviewObject, new Vector3(transform.position.x, 2.5f, hit.point.z), Quaternion.FromToRotation(Vector3.up, hit.normal), transform);
        return _preview;
    }

    private void OnMouseExit()
    {
        Destroy(_preview);
    }

    public void FinalizeBuy()
    {
        GameManager.Instance.playerResources -= _cost;
    }


    public bool HasTrap()
    {
        return (_trapGO != null) ? true : false;  // Return true if a trap exists
    }

    public bool HasPreview()
    {
        return (_preview != null) ? true : false;  // Return true if a trap exists
    }
}
