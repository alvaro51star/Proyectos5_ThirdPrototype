using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] InputManager InputManager;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InputManager.OnInteracting += Prueba; //suma la funcion da igual si reciba el input o no, despues input hara que las funciones del evento se hagan
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InputManager.OnInteracting -= Prueba; 
        }
    }

    protected virtual void Prueba()
    {
        Debug.Log("prueba evento");
    }

    private void OnDisable()
    {
        InputManager.OnInteracting -= Prueba;

    }
}
