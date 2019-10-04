using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacementController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] trapObjectPrefabs;

    private GameObject currentTrapObject;

    private float mouseWheelRotation;
    private int currentPrefabIndex = -1;
    // Update is called once per frame
    private void Update()
    {
        HandleNewObjectHotKey();

        if (currentTrapObject != null)
        {
            MoveCurrentObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
        }
    }
    private void HandleNewObjectHotKey()
    {
        for (int i = 0; i < trapObjectPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                if (PressedKeyOfCurrentPrefab(i))
                {
                    Destroy(currentTrapObject);
                    currentPrefabIndex = -1;
                }
                else
                {
                    if (currentTrapObject != null)
                    {
                        Destroy(currentTrapObject);
                    }
                    currentTrapObject = Instantiate(trapObjectPrefabs[i]);
                    currentPrefabIndex = i;
                }
                break;
            }
        }
    }

    private bool PressedKeyOfCurrentPrefab(int i)
    {
        return currentTrapObject != null && currentPrefabIndex == i;
    }

    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log(Input.mousePosition);
        Debug.Log(ray);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            currentTrapObject.transform.position = hitInfo.point;
            currentTrapObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
      
    }

    private void RotateFromMouseWheel()
    {
        //Debug.Log(Input.mouseScrollDelta);
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentTrapObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentTrapObject = null;
        }
    }
}
