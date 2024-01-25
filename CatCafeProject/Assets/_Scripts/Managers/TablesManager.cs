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
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CatMovement>() != null && other.GetComponent<ClientStates>().catState != CatState.Leaving)
        {
            catMovement = other.GetComponent<CatMovement>();
            catData = other.GetComponent<ClientData>().catType;
            catMovement.tablesManager = this;

            StartCoroutine(catMovement.WaitForMovementToAssignedTable());
        }
    }


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
                availableTablesList.Add(item);
            }
        }

        if(availableTablesList[0] == null)
        {
            return null;
        }

        foreach(var item in availableTablesList)//check if client can pass to available table
        {
            Transform destination = item.selectedChair;
            bool canPass = catMovement.CalculateNewPath(destination);
            if (!canPass)
            {
                CalculateUselessTables(item);
                availableTablesList.Remove(item);
            }
        }


        foreach(var item in catData.likes)//return available table of cat's theme
        {            
            foreach(var tiem in availableTablesList)
            {
                if (item == tiem.furnitureTheme)
                {
                    tiem.ResetTableData(true);
                    return tiem;
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
