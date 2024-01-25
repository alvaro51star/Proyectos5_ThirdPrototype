using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ClientStates>().catState == CatState.Leaving)
        {
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }
}
