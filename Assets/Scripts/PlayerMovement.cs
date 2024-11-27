using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float currentSpeed; // the current player speed
    public float moveSpeed = 5f; // speed when player is walking
    public float sprintSpeed = 10f; // speed when player is running
    public float mouseSensitivity = 2f;
    float verticalRotation = 0;
    Rigidbody rb;
    Animator animator;

    Transform camTransform; // Transform component of the camera
    public Transform playerHead; // object for the camera to follow
                                 // public float cameraOffset; // camera distance from player

    public Transform playerFeet;  // object positioned in the players feet position
    public LayerMask groundLayerMask;  // the ground layer mask

    //jump
    public float jumpForce = 10f;

    public int frame = 30;


    void Start()
    {
        
        currentSpeed = moveSpeed; // set the current speed to the walk speed
        // get component references
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked; // locking the cursor to the screen
        Cursor.visible = false;

        camTransform = Camera.main.transform; // getting reference to the camera transform
    }

    void Update()
    {
        if(GameManager.Instance.IsPaused)
            return;
        Rotation();
        SprintInput();
        Movement();

        Jump();
    }

    void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X"); // horizontal mouse movement
        float mouseY = Input.GetAxis("Mouse Y");  // vertical mouse movement

        float yRotation = mouseX * mouseSensitivity;
        float xRotation = mouseY * mouseSensitivity;
        transform.Rotate(0, yRotation, 0); // rotate the player in the Y axis

        verticalRotation -= xRotation;
        verticalRotation = Mathf.Clamp(verticalRotation, -10, 89); // clamp the rotation

        playerHead.localRotation = Quaternion.Euler(verticalRotation, 0, 0); // rotate the player head for the camera 
    }

    void Movement()
    {
        float sideInput = Input.GetAxis("Horizontal"); // A and D keys input
        float forwardInput = Input.GetAxis("Vertical");  // W and S keys input

        Vector3 movement = new Vector3(sideInput,0,forwardInput) * currentSpeed;
        movement = transform.TransformDirection(movement);

        rb.velocity = new Vector3(movement.x, rb.velocity.y,movement.z);

        // Set animator variable for animations
        animator.SetFloat("MoveSpeed", movement.magnitude);
    }


    void Jump()
    {
        // check a sphere in the player's feet with the ground layer, if true the player is grounded
        bool isGrounded = Physics.CheckSphere(playerFeet.position, 0.05f, groundLayerMask);
        // only jump if escape space bar is pressed and the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
           // rb.velocity = new Vector3(rb.velocity.x,jumpForce,rb.velocity.z); // add a vertical velocity
        }
    }

    void SprintInput() 
    {
        // if the player holds shift the current speed is set to the sprint speed
        // when the player releases the key, the speed is set to the walk speed
        // also change the animation speed, so its faster when the player is running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
            animator.SetFloat("AnimationSpeed", 1.5f);
        }
        else
        {
            currentSpeed = moveSpeed;
            animator.SetFloat("AnimationSpeed", 0.75f);
        }
    }
}
