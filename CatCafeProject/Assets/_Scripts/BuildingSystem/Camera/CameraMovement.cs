using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that moves and rotates a Cinemachine camera target to control the movement and rotation of the camera.
/// It also allows to zoom in and out using the scroolwheel
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [SerializeField, Min(0.1f)]
    private float speed = 1f;
    [SerializeField]
    private float rotationSpeed = 15;
    //This parameter is used for lerping
    [SerializeField]
    private float movementTime = 0.1f;
    [SerializeField]
    private Vector3 zoomAmount, zoomLimitClose, zoomLimitFar;

    [SerializeField]
    CinemachineVirtualCamera cameraReference;
    //We need the Transposer to implement Zoom operation
    CinemachineTransposer cameraTransposer;
    private Vector3 newZoom;

    private Quaternion targetRotation;

    Vector2 input;

    [SerializeField]
    private int constraintXMax = 5, constraintXMin = -5, constraintZMax = 5, constraintZMin = -5;

    private void Start()
    {
        //We need to use this Cinemachine specific method to access Transposer
        cameraTransposer = cameraReference.GetCinemachineComponent<CinemachineTransposer>();
        targetRotation = transform.rotation;
        newZoom = cameraTransposer.m_FollowOffset;
        
    }
    
    /// <summary>
    /// This code could be refactored to the Input class so that we can easily change the assigned buttons.
    /// Right now WSAD/Arrows controls the movement. Q and E allows to rotate the camera.
    /// Scroll wheel allows to zoom in and out
    /// </summary>
    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        int rotationDirection = 0;
        if(Input.GetKey(KeyCode.Q)) 
            rotationDirection = -1;
        if (Input.GetKey(KeyCode.E))
            rotationDirection = 1;
        targetRotation = transform.rotation* Quaternion.Euler(Vector3.up * rotationDirection * rotationSpeed);

        if(Mathf.Approximately(Input.mouseScrollDelta.y,0) == false)
        {
            //Checks which way we are scroling to decide if we want to zoom in or out
            int zoomDirection = Input.mouseScrollDelta.y > 0 ? 1 : -1;
            newZoom += zoomAmount * zoomDirection;
            newZoom = ClampVector(newZoom, zoomLimitClose, zoomLimitFar);
        }

        transform.position += (transform.forward * input.y + transform.right * input.x) * speed * Time.deltaTime;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, constraintXMin, constraintXMax),
            transform.position.y,
            Mathf.Clamp(transform.position.z, constraintZMin, constraintZMax));

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / movementTime);
        cameraTransposer.m_FollowOffset = Vector3.Lerp(cameraTransposer.m_FollowOffset, newZoom, Time.deltaTime / movementTime);

    }

    /// <summary>
    /// This allows us to clamp zoom of the camera to the limit values
    /// </summary>
    /// <param name="newZoom"></param>
    /// <param name="zoomLimitClose"></param>
    /// <param name="zoomLimitFar"></param>
    /// <returns></returns>
    private Vector3 ClampVector(Vector3 newZoom, Vector3 zoomLimitClose, Vector3 zoomLimitFar)
    {
        newZoom.x = Mathf.Clamp(newZoom.x, zoomLimitClose.x, zoomLimitFar.x);
        newZoom.y = Mathf.Clamp(newZoom.y,zoomLimitClose.y,zoomLimitFar.y);
        newZoom.z = Mathf.Clamp(newZoom.z, zoomLimitClose.z, zoomLimitFar.z);
        return newZoom;
    }

    //private void FixedUpdate()
    //{
    //    transform.position += (transform.forward * input.y + transform.right * input.x) * speed*Time.fixedDeltaTime;

    //    transform.position = new Vector3(
    //        Mathf.Clamp(transform.position.x, constraintXMin, constraintXMax), 
    //        transform.position.y, 
    //        Mathf.Clamp(transform.position.z, constraintZMin, constraintZMax));

    //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / movementTime);
    //    cameraTransposer.m_FollowOffset = Vector3.Lerp(cameraTransposer.m_FollowOffset, newZoom, Time.deltaTime / movementTime);
    //}
}
