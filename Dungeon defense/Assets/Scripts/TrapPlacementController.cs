using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacementController : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager = default;  // Used for tooltips
    [SerializeField] private Trap[] _trapList = default;  // List of the class TrapData, holding all traps and their properties

    private GameObject _tempTrapGO;                   // A temporary trap object to track the previews
    private Trap _tempTrap;                   // A temporary trap object to track the previews
    private int _tempTrapIndex = -1;                // Temporary index showing what trap we're working with
    private Spawnable _spawnTile;                      // The target spawn point
    private RaycastHit _hit;                        // The raycast pointing towards the spawn point
    private bool _sell = false;                     // Checks whether or not an object is ready to be sold

    private void Update()
    {
        HandleNewObjectHotKey();
        FinalizeOnClick();
        UpdateTile();
    }


    private Spawnable FindSpawn()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _hit))      // Check if the target is a spawn tile
            return (_hit.transform.gameObject.GetComponent<Spawnable>()) ? _hit.transform.gameObject.GetComponent<Spawnable>() : null;
        else
            return null;
    }

    private void FindIndex()
    {
        for (int i = 0; i < _trapList.Length; i++)
            if (_trapList[i] == _tempTrap)
                _tempTrapIndex = i;
    }

    private void HandleNewObjectHotKey()
    {
        for (int i = 0; i < _trapList.Length; i++) // Goes through 0-9, depending on the amount of traps you can spawn
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                Spawnable lookAt = FindSpawn();
                if (lookAt != null && lookAt.HasTrap()) // Prepare to sell the trap if there already exists one
                {
                    _uiManager.ToggleSpawnTooltip(lookAt.Trap.Price, BuySellBroke.Sell);  // Tooltips
                    _sell = true;
                    _tempTrapGO = lookAt.Trap.TrapObject;
                    _tempTrap = lookAt.Trap;
                    FindIndex();
                    _spawnTile = lookAt;
                    break;
                }

                if (_spawnTile == null)
                    _spawnTile = FindSpawn();            // Find the target
                if (_spawnTile != null)              // Make sure there is a target
                {
                    if (KeyPressedAgain(i))       // Press a key once for preview, then again to exit
                    {
                        _spawnTile.DestroyTrap();
                        _tempTrapIndex = -1;
                        _uiManager.TurnOffSpawnTooltip();
                    }
                    else
                    {
                        if (GameManager.Instance.playerResources < _trapList[i].Price) // Make sure you have enough resources
                            _uiManager.ToggleSpawnTooltip(_trapList[i].Price, BuySellBroke.Broke);
                        else
                        {
                            if (TagMatchingType(i))     // Make sure you only get the tooltip on valid traps
                                _uiManager.ToggleSpawnTooltip(_trapList[i].Price, BuySellBroke.Buy);

                            // Create a temporary object at the hit position
                            _tempTrapGO = _spawnTile.PreviewTrap(_trapList[i], _hit);
                            _tempTrap = _trapList[i];
                            _sell = false;
                            _tempTrapIndex = i;
                        }
                    }
                }
                break;
            }
        }
    }

    private bool TagMatchingType(int i)
    {
        if (((_hit.transform.gameObject.tag == "xWall" || _hit.transform.gameObject.tag == "zWall") && _trapList[i].Type == TrapType.Wall)
            || (_hit.transform.gameObject.tag == "Floor" && _trapList[i].Type == TrapType.Floor))
            return true;
        else
            return false;
    }

    private void UpdateTile()
    {
        if (_spawnTile != null && !_spawnTile.HasPreview() && _sell == false)
        {
            _spawnTile = null;
            _uiManager.TurnOffSpawnTooltip();
        }
    }

    private bool KeyPressedAgain(int i)
    {
        return _tempTrapGO != null && _tempTrapIndex == i;
    }

    private void FinalizeOnClick()
    {
        if (Input.GetMouseButtonDown(0))
            if (_spawnTile != null)
            {
                if (_sell)
                {
                    _spawnTile.DestroyTrap(true);
                    _sell = false;
                    _uiManager.TurnOffSpawnTooltip();
                }
                else if (_spawnTile.HasPreview() && _sell == false)
                {
                    _spawnTile.AddTrap(_tempTrap, _hit);
                    _spawnTile.FinalizeBuy();
                    _uiManager.TurnOffSpawnTooltip();
                }
                _spawnTile = null;
                // Reset the current trap object so the player can place new ones
                _tempTrapGO = null;
                _tempTrapIndex = -1;
                _tempTrap = null;

            }
    }

    public Trap GetTrapData(GameObject trap)
    {
        for (int i = 0; i < _trapList.Length; i++)
            if (_trapList[i].TrapObject == trap)
                return _trapList[i];
        return null;
    }
}
