using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;

    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

    float fallSpeed;

    string gravityDirection;

    int playerGravityRotate = 0;

    bool changeDirection;

    CameraController cameraController;

    Animator animator;

    CharacterController characterController;

    Quaternion targetRotation;
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();  
    }
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        var moveInput = (new Vector3(horizontal, 0, vertical).normalized);

        var moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();

        var velocity = moveDir * moveSpeed * -1;

        if (isGrounded)
        {
            fallSpeed = -0.5f;
            animator.SetBool("onGround", true);
        }

        if (!isGrounded)
        {
            animator.SetBool("onGround", false);
            if(!changeDirection)
            {
                fallSpeed += Physics.gravity.y * Time.deltaTime;
                velocity.y = fallSpeed;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {           
            gravityDirection = "left";
            playerGravityRotate -= 90;
            changeDirection = true;          

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gravityDirection = "right";
            playerGravityRotate += 90;
            changeDirection = true;
        }


        else if (changeDirection)
        {
            transform.rotation = Quaternion.Euler(0, 180, playerGravityRotate);
            /*if ((playerGravityRotate / 180) % 2 != 0)
            {
                Physics.gravity = new Vector3(0, -9.81f, 0);
                fallSpeed += Physics.gravity.y * Time.deltaTime;
                velocity.y = fallSpeed;
            }

            else if ((playerGravityRotate / 180) % 2 == 0)
            {
                Physics.gravity = new Vector3(0, 9.81f, 0);
                fallSpeed += Physics.gravity.y * Time.deltaTime;
                velocity.y = fallSpeed;
            }*/

            switch (gravityDirection)
            {
                case "left":                               
                    Physics.gravity = new Vector3(9.81f, 0, 0);
                    fallSpeed += Physics.gravity.x * Time.deltaTime;
                    velocity.x = fallSpeed;
                    break;

                case "right":
                    Physics.gravity = new Vector3(-9.81f, 0, 0);
                    fallSpeed += Physics.gravity.x * Time.deltaTime;
                    velocity.x = fallSpeed;
                    break;
            }
       
        }
       
        characterController.Move(velocity * Time.deltaTime);
        //rb.AddForce(velocity);

        if (moveAmount > 0)
        {          
            targetRotation = Quaternion.LookRotation(moveDir*-1);
        }
       
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);

    }

    void GroundCheck()
    {
       isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}
