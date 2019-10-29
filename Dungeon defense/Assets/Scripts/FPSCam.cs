using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCam : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 2f;
    private Vector2 _mouseLook;
    private GameObject _character;
    // Start is called before the first frame update
    void Start()
    {
        _character = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");
        Vector2 look = new Vector2(horizontal, vertical);
        _mouseLook += look * _sensitivity;
        _mouseLook.y = Mathf.Clamp(_mouseLook.y, -80f, 80);
        transform.localRotation = Quaternion.AngleAxis(-_mouseLook.y, Vector3.right);
        _character.transform.localRotation = Quaternion.AngleAxis(_mouseLook.x, _character.transform.up);
    }
}
