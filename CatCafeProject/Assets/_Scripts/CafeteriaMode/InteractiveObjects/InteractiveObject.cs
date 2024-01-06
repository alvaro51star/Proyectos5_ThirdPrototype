using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{ //solamente se usara como padre para interactuar
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InputManager.OnInteracting += Interaction;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InputManager.OnInteracting -= Interaction;
        }
    }

    protected virtual void Interaction()
    {

    }

    private void OnDisable()
    {
        InputManager.OnInteracting -= Interaction;
    }
}
