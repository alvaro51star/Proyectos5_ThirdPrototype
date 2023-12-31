using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public delegate void PlayerInside();
    public static event PlayerInside OnPlayerInside;

    public delegate void PlayerOutside();
    public static event PlayerOutside OnPlayerOutSide;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (OnPlayerInside != null)
            {
                OnPlayerInside();
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (OnPlayerOutSide != null)
            {
                OnPlayerOutSide();
            }
        }
    }
}
