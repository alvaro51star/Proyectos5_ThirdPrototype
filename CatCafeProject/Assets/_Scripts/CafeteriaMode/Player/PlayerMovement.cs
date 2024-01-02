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

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        GetInputDirection();

        Movement();
    }


    private void Movement()
    {
        if (canMove)
        {
            characterController.SimpleMove(directionInput.normalized * speed);
        }
        //else anim de idle
    }

    private Vector3 GetInputDirection()//cambiar esto segun eventos?
    {
        directionX = Input.GetAxisRaw("Horizontal");
        directionZ = Input.GetAxisRaw("Vertical");       
        directionInput = new Vector3(directionX, 0, directionZ);

        return directionInput;
    }    
}
