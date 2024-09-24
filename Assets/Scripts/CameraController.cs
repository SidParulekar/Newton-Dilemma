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

        var targetRotation = Quaternion.Euler(rotationX*-1, rotationY + 180, 0);
        var cameraPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);
        transform.position = cameraPosition + Quaternion.Euler(rotationX, rotationY, 0) * new Vector3(0, cameraHeight, cameraDistance);
        transform.rotation = targetRotation;
    }
}
