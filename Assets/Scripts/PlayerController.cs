using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    InputManager inputManager;
    Vector3 moveDirection;

    Transform cameraObject;

    Rigidbody playerRigidBody;


    [SerializeField] float movementSpeed = 5f;

    [SerializeField] float rotationSpeed = 15f;

    [SerializeField] Quaternion currentRotation;

    int playerGravityRotate = 0;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>(); 
        playerRigidBody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        //animator.SetBool("onGround", true);
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
        //HandleGravityChange();
    }
    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;

        moveDirection.Normalize();

        moveDirection.y = 0;

        moveDirection *= movementSpeed;

        Vector3 movementVelocity = moveDirection;

        playerRigidBody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;     

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;

        targetDirection.Normalize();

        targetDirection.y = 0;
                                  
        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        targetRotation.z = HandleGravityChange();
        currentRotation = targetRotation;
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed*Time.deltaTime);
        //playerRotation.z = HandleGravityChange();
        transform.rotation = playerRotation;
        
    }

    private int HandleGravityChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {           
          playerGravityRotate -= 90;        
        }

        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            playerGravityRotate += 90;            
        }    
        return playerGravityRotate;

    }
}
