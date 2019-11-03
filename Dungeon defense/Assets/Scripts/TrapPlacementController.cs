using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type { Floor, Wall, All };   // Type let's you know where a trap can be placed

[System.Serializable]
public class TrapData // Holds information about each individual trap
{
    public GameObject objectToSpawn;        // The trap you want to spawn
    public int costToBuild;                 // The resource cost of the trap
    public Type type;                       // Where the trap can be placed
}

public class TrapPlacementController : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;  // Used for tooltips
    [SerializeField] private TrapData[] _trapList;  // List of the class TrapData, holding all traps and their properties
    private GameObject _tempTrap;                   // A temporary trap object to track the previews
    private int _tempTrapIndex = -1;                // Temporary index showing what trap we're working with
    private Spawnable _target;                      // The target spawn point
    private RaycastHit _hit;                        // The raycast pointing towards the spawn point
    private bool _sell = false;                     // Checks whether or not an object is ready to be sold

    private void Update()
    {
        HandleNewObjectHotKey();
        FinalizeOnClick();
    }


    private void FindSpawn()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _hit))
            // Check if the target is a spawn tile
            _target = (_hit.transform.gameObject.GetComponent<Spawnable>()) ? _hit.transform.gameObject.GetComponent<Spawnable>() : null;
        else
            _target = null;
    }

    private void HandleNewObjectHotKey()
    {
        for (int i = 0; i < _trapList.Length; i++)
        {
            // Goes through 0-9, depending on the amount of traps you can spawn
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                FindSpawn();                      // Find the target
                if (_target != null)              // Make sure there is a target
                    if (KeyPressedAgain(i))       // Press a key once for preview, then again to exit
                    {
                        _target.DestroyTrap();
                        _tempTrapIndex = -1;
                        _uiManager.TurnOffSpawnTooltip();
                    } 
                    else
                    {
                        if (_tempTrap != null)
                        {  // If you're changing target without having bought the previous one
                            _target.DestroyTrap();
                            Destroy(_tempTrap);
                        }

                        if (_target.HasTrap()) // Prepare to sell the trap if there already exists one
                        {
                            _uiManager.ToggleSpawnTooltip(_trapList[i].costToBuild, BuySellBroke.Sell);  // Tooltips
                            _sell = true;
                            _tempTrapIndex = i;
                        }
                        else if (GameManager.Instance.playerResources < _trapList[i].costToBuild) // Make sure you have enough resources
                            _uiManager.ToggleSpawnTooltip(_trapList[i].costToBuild, BuySellBroke.Broke);
                        else
                        {
                            if (TagMatchingType(i))     // Make sure you only get the tooltip on valid traps
                                _uiManager.ToggleSpawnTooltip(_trapList[i].costToBuild, BuySellBroke.Buy);

                            // Create a temporary object at the hit position
                            _tempTrap = _target.AddTrap(_trapList[i], _hit);
                            _sell = false;
                            _tempTrapIndex = i;
                        }
                    }
                break;
            }
        }
    }

    private bool TagMatchingType(int i)
    {
        if (((_hit.transform.gameObject.tag == "xWall" || _hit.transform.gameObject.tag == "yWall") && _trapList[i].type == Type.Wall)
            || (_hit.transform.gameObject.tag == "Floor" && _trapList[i].type == Type.Floor))
            return true;
        else
            return false;
    }

    private bool KeyPressedAgain(int i)
    {
        return _tempTrap != null && _tempTrapIndex == i;
    }

    private void FinalizeOnClick()
    {
        if (Input.GetMouseButtonDown(0))
            if (_target != null)
            {
                if (_sell)
                {
                    _target.DestroyTrap(true);
                    _sell = false;
                    _uiManager.TurnOffSpawnTooltip();
                }
                else if (_target.HasTrap() && _sell == false)
                {
                    _target.FinalizeBuy();
                    _target = null;
                    _uiManager.TurnOffSpawnTooltip();
                }
                // Reset the current trap object so the player can place new ones
                _tempTrap = null;

            }
    }

    public TrapData GetTrapData(GameObject trap)
    {
        for (int i = 0; i < _trapList.Length; i++)
            if (_trapList[i].objectToSpawn == trap)
                return _trapList[i];
        return null;
    }
}
