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
        List<TableData> availableTablesList = new();

        foreach(var item in tableDataList)//check not occupied tables
        {
            if(!item.isOccupied)
            {
                Debug.Log("for de si estan ocupadas");
                availableTablesList.Add(item);
            }
        }

        if(availableTablesList[0] == null)
        {
            return null;
        }

        for (int i = 0; i < availableTablesList.Count; i++)//check if client can pass to available table
        {
            Transform destination = tableDataList[i].selectedChair;
            bool canPass = catMovement.CalculateNewPath(destination);
            if (!canPass)
            {             
                CalculateUselessTables(tableDataList[i]);
                availableTablesList.RemoveAt(i);
            }
        }

        for(int i = 0; i < catData.likes.Count; i ++)//return available table of cat's theme
        {
            for(int j = 0; j < availableTablesList.Count; j++)
            {
                if(catData.likes[i] == availableTablesList[j].furnitureTheme)
                {
                    availableTablesList[j].ResetTableData(true);

                    Debug.Log("doble for");

                    return availableTablesList[j];
                }
            }
        }

        List<float> distanceList = new List<float>(availableTablesList.Count);
        float distance = Vector3.Distance(this.gameObject.transform.position, availableTablesList[0].selectedChair.position);
        float smallestDistance = distance;
        int index = 0;
        for (int i = 0; i < availableTablesList.Count; i++) //return closest available table
        {
            distance = Vector3.Distance(this.gameObject.transform.position, availableTablesList[i].selectedChair.position);
            if (distance < smallestDistance)
            {
                index = i;
                smallestDistance = distance;
            }
        }

        availableTablesList[index].ResetTableData(true);
        return availableTablesList[index];        
    }

}
