using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public static ClientManager instance;

    [SerializeField] private Dictionary<Transform, bool> queueSlots = new();
    [SerializeField] private List<Transform> queueList;

    [SerializeField] private Transform parent;
    [SerializeField] private List<GameObject> clients;

    [SerializeField] private float timeBetweenCats = 1f;
    [SerializeField] private int nextClient = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        CatMovement.OnTableAssigned += QueueUpdate;
    }

    private void OnDisable()
    {
        CatMovement.OnTableAssigned -= QueueUpdate;
    }

    private void Start()
    {
        for (int i = 0; i < queueList.Count; i++)
        {
            queueSlots.Add(queueList[i], false);
        }
    }

    private void ShowQueue()
    {
        foreach (var item in clients)
        {
            Debug.Log(item);
        }
    }

    private void ShowDictionary()
    {
        foreach (var item in queueSlots)
        {
            Debug.Log($"{item.Key} = {item.Value}");
        }
    }

    public void SpawnCats()
    {
        for (int i = 0; i < GameManager.instance.catsForTheDay.Count; i++)
        {
            GameObject currentCat = Instantiate(GameManager.instance.catsForTheDay[i].catPrefab, parent);
            currentCat.SetActive(false);
            clients.Add(currentCat);
        }
    }

    /// <summary>Use this to destroy all the cats in scene</summary>
    public void DestroyCats()
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform cat = parent.GetChild(i);
            if (cat.gameObject != null)
            {
                Destroy(cat.gameObject);
            }
        }
    }

    public IEnumerator TestCats()
    {
        for (int i = 0; i < queueSlots.Count; i++)
        {
            if (i >= clients.Count)
                break;
            Transform transformValue = queueSlots.ElementAt(i).Key;
            bool boolValue = queueSlots.ElementAt(i).Value;
            if (boolValue == false)
            {
                clients[i].SetActive(true);
                clients[i].GetComponent<CatMovement>().MovementToDestination(transformValue);
                queueSlots[transformValue] = true;
            }
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }

    private void QueueUpdate()
    {
        if (clients.Count <= 1)
        {
            return;
        }
        queueSlots[queueList[0]] = false;
        clients.RemoveAt(0);

        clients[0].GetComponent<CatMovement>().MovementToDestination(queueList[0]);
        queueSlots[queueList[1]] = false;
        queueSlots[queueList[0]] = true;

        int i;
        for (i = 1; i < queueSlots.Count; i++)
        {
            if (i <= clients.Count)
            {
                clients[i - 1].GetComponent<CatMovement>().MovementToDestination(queueList[i - 1]);
                queueSlots[queueList[i]] = false;
                queueSlots[queueList[i - 1]] = true;
            }
        }
        if (i >= clients.Count)
        {

        }
        else if (clients[i] != null && clients[i].activeSelf)
        {
            clients[i].GetComponent<CatMovement>().MovementToDestination(queueList[i - 1]);
            queueSlots[queueList[i]] = false;
            queueSlots[queueList[i - 1]] = true;
        }

        if (queueSlots[queueList[^1]] == false)
        {
            GameObject nextCat = LookForFirstClientInactive();
            if (nextCat != null)
            {
                nextCat.SetActive(true);
                nextCat.GetComponent<CatMovement>().MovementToDestination(queueList[^1]);
                queueSlots[queueList[^1]] = true;
            }
        }
    }

    private GameObject LookForFirstClientInactive()
    {
        for (int i = 0; i < clients.Count; i++)
        {
            if (clients[i].activeSelf == false)
            {
                return clients[i];
            }
        }
        return null;
    }

    private Transform LookForFirstPositionFree()
    {
        foreach (var item in queueSlots)
        {
            if (item.Value == false)
            {
                return item.Key;
            }
        }

        return null;
    }

    public void QueueReset()
    {
        for (int i = 0; i < queueSlots.Count; i++)
        {
            queueSlots[queueList[i]] = false;
        }
    }

    public void ClientReset()
    {
        clients.Clear();
    }

}
