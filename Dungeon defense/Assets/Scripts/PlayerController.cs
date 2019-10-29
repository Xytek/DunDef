using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 250f;
    private CharacterInput _characterInput;
    private Rigidbody _mybody;
    private Animator _anim;
    private bool _isGrounded;
    private bool _canJump;

    void Start()
    {
        _characterInput = GetComponent<CharacterInput>();
        if (_characterInput == null)
        {
            Debug.LogError("Character input is null");
        }
        _mybody = GetComponent<Rigidbody>();
        if (_mybody == null)
        {
            Debug.LogError("Character rigid body is null");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Character animation is null");
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 moveDirection = new Vector3(horizontal, 0.0f, vertical) * _speed * Time.deltaTime;

        transform.Translate(moveDirection);
        if (horizontal != 0 || vertical != 0)
            _anim.SetBool("isIdle", false);
        else
            _anim.SetBool("isIdle", true);


        // Creates a sphere below the character that checks whether or not it's overlapping with the floor
        _isGrounded = Physics.CheckSphere(_groundCheck.transform.position, 0.05f);
        if (Input.GetKeyDown(_characterInput.jumpKey) && _isGrounded)
            _canJump = !_canJump;

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
    }

    void FixedUpdate()
    {
        if (_canJump)
        {
            _mybody.AddForce(Vector3.up * _jumpForce);
            _canJump = !_canJump;
        }
    }
}