using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueSlotsTrigger : MonoBehaviour
{
    public bool isOccupied = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CatMovement>())
        {
            isOccupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CatMovement>() && isOccupied)
        {
            isOccupied = false;
        }
    }
}
