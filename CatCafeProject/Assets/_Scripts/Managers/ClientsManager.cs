using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientsManager : MonoBehaviour
{//manages the queue
    [SerializeField] private QueueSlotsTrigger[] queueSlots;
    [SerializeField] private GameObject parent;
    public List<CatMovement> clientPrefabs = new List<CatMovement>();
    [SerializeField] private int numberOfClients;   

}
