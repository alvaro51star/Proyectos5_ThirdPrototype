using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClientsManager : MonoBehaviour
{//manages the queue
    [SerializeField] private QueueSlotsTrigger[] queueSlots;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform atrilTransform;
    [SerializeField] private int numberOfClients;   

    private List<GameObject> clients = new List<GameObject>();
    private List<GameObject> catsInQueue = new List<GameObject>();
    private bool cafeteriaMode = false;

    private void Start()
    {
        parent = queueSlots[queueSlots.Length - 1].transform;

        if(GameManager.instance.initialGameMode == GameModes.Cafeteria)
        {
            OnCafeteriaGameMode();
        }
    }
   
    private void OnCafeteriaGameMode()//llamarlo por evento al cambiar el modo de juego
    {
        InstantiateAndDisableClients();
        cafeteriaMode = true;
    }

    private void InstantiateAndDisableClients()
    {
        numberOfClients = GameManager.instance.catsForTheDay.Count;

        for (int i = 0; i < numberOfClients; i++)
        {
            GameObject gameObject = Instantiate(GameManager.instance.catsForTheDay[i].catPrefab, parent);
            gameObject.SetActive(false);
            clients.Add(gameObject);
        }
    }   

    private IEnumerator EnableClientsIfLastSlotIsNotOccupied()
    {
        if (!queueSlots[queueSlots.Length - 1].isOccupied)
        {
            for(int i = 0; i < numberOfClients; i++)
            {
                if (!queueSlots[queueSlots.Length - 1].isOccupied)
                {
                    if (!clients[i].gameObject.activeSelf)
                    {
                        catsInQueue.Add(clients[i]);
                        Debug.Log(i);

                        catsInQueue[i].gameObject.SetActive(true);
                        queueSlots[queueSlots.Length - 1].isOccupied = true;

                        Debug.Log("client " + catsInQueue[i].gameObject.activeSelf);
                        Debug.Log("last queueSlot " + queueSlots[queueSlots.Length - 1].isOccupied);

                        yield return new WaitForSeconds(1f);

                    }
                }
                else
                    break;

            }
        }      
    }

    private void ClientsMovement()
    {
        if(catsInQueue.Count > 0)
        {
            for (int i = 0; i < catsInQueue.Count; i++)
            {
                for (int j = 0; j < queueSlots.Length - 1; j++)
                {
                    if (!queueSlots[queueSlots.Length - 1 - j].isOccupied)
                    {
                        if (!queueSlots[0].isOccupied)
                        {
                            Debug.Log("primer slot libre");

                            catsInQueue[i].GetComponent<CatMovement>().MovementToDestination(atrilTransform);
                            catsInQueue.Remove(catsInQueue[i]);

                            queueSlots[0].isOccupied = true;
                            return;
                        }

                        else if (queueSlots[queueSlots.Length - 1 - j] != queueSlots[0])
                        {
                            queueSlots[j].isOccupied = true;

                            Transform destination = queueSlots[queueSlots.Length - 1 - j].transform;
                            catsInQueue[i].GetComponent<CatMovement>().MovementToDestination(destination);
                        }

                    }
                }              
            }
        }
        
    }

    private void Update()
    {
        if(cafeteriaMode && (clients.Count == numberOfClients))
        {
            StartCoroutine(EnableClientsIfLastSlotIsNotOccupied());
            if(catsInQueue.Count > 0)
            {
                ClientsMovement();
            }
        }
    }
}
