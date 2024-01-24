using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [SerializeField] private Dictionary<Transform, bool> queueSlots = new();
    [SerializeField] private List<Transform> queueList;

    [SerializeField] private Transform parent;
    [SerializeField] private List<GameObject> clients;

    [SerializeField] private float timeBetweenCats = 1f;

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
        SpawnCats();
        StartCoroutine(TestCats());
    }


    private void ShowDictionary()
    {
        foreach (var item in queueSlots)
        {
            Debug.Log($"{item.Key} = {item.Value}");
        }
    }

    private void SpawnCats()
    {
        for (int i = 0; i < GameManager.instance.catsForTheDay.Count; i++)
        {
            GameObject currentCat = Instantiate(GameManager.instance.catsForTheDay[i].catPrefab, parent);
            currentCat.SetActive(false);
            clients.Add(currentCat);
        }
    }

    private void TestFirstCat()
    {
        clients[0].SetActive(true);
        clients[0].GetComponent<CatMovement>().MovementToDestination(queueSlots.ElementAt(0).Key);
    }

    private IEnumerator TestCats()
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

        ShowDictionary();

        yield return null;
    }

    private void QueueUpdate()
    {
        queueSlots[queueList[0]] = false;

        for (int i = 1; i < queueSlots.Count; i++)
        {
            if(i >= clients.Count)
                break;
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

}
