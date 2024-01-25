using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool canMove = true; //para facilitar pausa
    [SerializeField] private float speed;
    private Vector3 directionInput;
    private float directionX, directionZ;
    private CharacterController characterController;

    private Animator cmpPlayerAnimator;
    public float rotationVe;
    public Transform body;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cmpPlayerAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        GetInputDirection();

        Movement();
        Rotate();
    }


    private void Movement()
    {
        if (canMove)
        {
            characterController.SimpleMove(directionInput.normalized * speed);
        }
        cmpPlayerAnimator.SetFloat("SpeedX", directionX);
        cmpPlayerAnimator.SetFloat("SpeedZ", directionZ);
    }

    private Vector3 GetInputDirection()//cambiar esto segun eventos?
    {
        directionX = Input.GetAxisRaw("Horizontal");
        directionZ = Input.GetAxisRaw("Vertical");       
        directionInput = new Vector3(directionX, 0, directionZ);

        return directionInput;
    }

    private void Rotate()
    {
        Vector3 movDirection = new Vector3(directionX, 0, directionZ);
        if (movDirection != Vector3.zero)
        {
            Quaternion wishedRotation = Quaternion.LookRotation(movDirection, Vector3.up);
            body.rotation = Quaternion.RotateTowards(body.rotation, wishedRotation, rotationVe * Time.deltaTime);
        }
    }
}
