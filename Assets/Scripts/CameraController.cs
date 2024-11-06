using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    [SerializeField] float cameraDistance = 5;
    [SerializeField] float cameraHeight = 2;

    [SerializeField] float rotationSpeed = 2;

    [SerializeField] float minVerticalAngle = -45;
    [SerializeField] float maxVerticalAngle = 45;

    [SerializeField] Vector2 framingOffset;

    int cameraRotateZ = 0;


    float rotationX;
    float rotationY;

    private void Start()
    {
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        rotationX += Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationY += Input.GetAxis("Mouse X");

        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cameraRotateZ -= 90;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cameraRotateZ += 90;
        }

        var targetRotation = Quaternion.Euler(rotationX*-1, rotationY + 180, cameraRotateZ);
        var cameraPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);
        transform.position = cameraPosition + Quaternion.Euler(rotationX, rotationY, 0) * new Vector3(0, cameraHeight, cameraDistance);
        transform.rotation = targetRotation;
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, cameraRotateZ);
}
