using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TablesManager : MonoBehaviour
{
    [SerializeField] private float secondsClientWaits;
    private CatMovement catMovement;
    private CatDataSO catData;
    public List<GameObject> tableList; //de momento usar esta, realmente usar las de FurnitureManager
    public List<TableData> tableDataList;

    private void Start()
    {
        if (GameManager.instance.initialGameMode == GameModes.Cafeteria)//esto ahora para debug
        {
            for (int i = 0; i < tableList.Count; i++)
            {
                tableDataList.Add(tableList[i].GetComponent<TableData>());
                tableDataList[i].tablesManager = this;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CatMovement>() != null)
        {
            catMovement = other.GetComponent<CatMovement>();
            catData = other.GetComponent<ClientData>().catType;
            catMovement.tablesManager = this;

            //if cat es primero en cola
            StartCoroutine(catMovement.WaitForMovementToAssignedTable());
        }
    }

    // private IEnumerator OnTriggerStay(Collider other)
    // {
    //     yield return new WaitForSeconds(2f);
    //     if(other.GetComponent<CatMovement>() != null)
    //     {
    //         catMovement = other.GetComponent<CatMovement>();
    //         catData = other.GetComponent<ClientData>().catType;
    //         catMovement.tablesManager = this;
    //         //if cat es primero en cola
    //         StartCoroutine(catMovement.WaitForClientMovement());
    //     }
    // }

    private void CalculateUselessTables(TableData uselessTable) //calculate number of tables that the client cant reach (no path available)
    {
        tableDataList.Remove(uselessTable);

        if (tableDataList.Count <= 0)
        {
            Debug.Log("El cliente no puede pasar a ninguna mesa");
        }
    }

    public TableData CheckAvailableTables()//en futuro llamado por evento en TableData
    {
        List<TableData> availableTableDataList = new List<TableData>();

        for (int i = 0; i < tableDataList.Count; i++)
        {
            Transform destination = tableDataList[i].selectedChair;
            bool canPass = catMovement.CalculateNewPath(destination);
            if (canPass)
            {
                if (!tableDataList[i].isOcupied)
                {
                    availableTableDataList.Add(tableDataList[i]);
                }
            }
            else
            {
                CalculateUselessTables(tableDataList[i]);
            }
        }

        for (int i = 0; i < catData.likes.Count; i++)//preferencia hacia mesas de la tematica que le guste
        {
            for (int j = 0; j < availableTableDataList.Count; j++)
            {
                if (catData.likes[i] == availableTableDataList[j].furnitureTheme)
                {
                    return availableTableDataList[j];
                }
            }
        }

        List<float> distanceList = new List<float>(availableTableDataList.Count);
        float distance = Vector3.Distance(this.gameObject.transform.position, availableTableDataList[0].selectedChair.position);
        float smallestDistance = distance;
        int index = 0;
        for (int i = 0; i < availableTableDataList.Count; i++) //preferencia hacia la mesa mas cercana
        {
            distance = Vector3.Distance(this.gameObject.transform.position, availableTableDataList[i].selectedChair.position);
            if (distance < smallestDistance)
            {
                index = i;
                smallestDistance = distance;
            }
        }
        return availableTableDataList[index];

        // if (availableTableDataList.Count > 0)
        // {
        //     return availableTableDataList[0];
        // }
        // else
        // {
        //     return null;
        // }
    }

}
