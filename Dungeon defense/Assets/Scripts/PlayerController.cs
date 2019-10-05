using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterInput characterInput;
    private Rigidbody mybody;
    private Animator animator;
    public Transform groundCheck;

    public float speed;
    public float jumpForce;

    public bool isGrounded;
    private bool canJump;

    // Start is called before the first frame update
    void Start()
    {
        characterInput = GetComponent<CharacterInput>();
        mybody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 moveDirection = new Vector3(horizontal, 0.0f, vertical) * speed * Time.deltaTime;

        transform.Translate(moveDirection);
        if (horizontal != 0 || vertical != 0)
        {
            animator.SetBool("isIdle", false);
        }
        else
        {
            animator.SetBool("isIdle", true);
        }


        // Creates a sphere below the character that checks whether or not it's overlapping with the floor
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, 0.05f);
        if (Input.GetKeyDown(characterInput.jumpKey) && isGrounded)
        {
            canJump = !canJump;
        }
    }

    void FixedUpdate()
    {
        if (canJump)
        {
            mybody.AddForce(Vector3.up * jumpForce);
            canJump = !canJump;
        }
    }
}