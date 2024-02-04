using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientDestroyer : MonoBehaviour
{
    private int clientsDestroyed = 0;
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        Destroy(other.gameObject);
        clientsDestroyed++;
        if (clientsDestroyed == GameManager.instance.catsForTheDay.Count)
        {
            EconomyManager.instance.PrintReceipt();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ResetClientDestroyed()
    {
        clientsDestroyed = 0;
    }
}
