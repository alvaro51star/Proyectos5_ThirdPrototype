using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Progress;

public class TablesManager : MonoBehaviour
{
    [SerializeField] private UIManager UImanager;
    [SerializeField] private float secondsClientWaits;
    private CatMovement catMovement;
    private CatDataSO catData;
    public List<GameObject> tableList; //de momento usar esta, realmente usar las de FurnitureManager
    public List<TableData> tableDataList;
    private List<TableData> unavailableTableDataList = new();

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

    private void OnDisable()
    {
        tableDataList.Clear();
        tableDataList.Clear();
        unavailableTableDataList.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CatMovement>() != null && other.GetComponent<ClientStates>().catState != CatState.Leaving)
        {
            catMovement = other.GetComponent<CatMovement>();
            catData = other.GetComponent<ClientData>().catType;
            catMovement.tablesManager = this;

            if(CheckCanPassAllTables())
            {
                StartCoroutine(catMovement.WaitForMovementToAssignedTable());
            }
        }
    }

    private bool CheckCanPassAllTables()
    {
        foreach(var table in tableDataList)
        {
            Transform destination = table.selectedChair;
            bool canPass = catMovement.CalculateNewPath(destination);
            if (!canPass)
            {
                //CalculateUselessTables(table);

                unavailableTableDataList.Add(table);

                if (unavailableTableDataList.Count == tableDataList.Count)
                {
                    Debug.Log("no se puede pasar a las mesas");
                    //GameManager.instance.UIManager.IsInGame(false);
                    //GameManager.instance.UIManager.ActivateUIGameObjects(GameManager.instance.UIManager.blockMenu, true);
                    UImanager.IsInGame(false);
                    UImanager.ActivateUIGameObjects(UImanager.blockMenu, true);
                    return false;
                }
            }
        }
        return true;
    }

    private void CalculateUselessTables(TableData uselessTable) //calculate number of tables that the client cant reach (no path available)
    {
        //tableDataList.Remove(uselessTable);

        
    }

    public TableData CheckAvailableTables()
    {
        List<TableData> availableTablesList = new();

        foreach(var item in tableDataList)//check not occupied tables
        {
            if(!item.isOccupied)
            {
                availableTablesList.Add(item);
            }
        }

        bool isNullOrEmpty = availableTablesList?.Any() != true;
        if (isNullOrEmpty)
        {
            return null;
        }

        /*foreach(var item in availableTablesList)//check if client can pass to available table
        {
            Transform destination = item.selectedChair;
            bool canPass = catMovement.CalculateNewPath(destination);
            if (!canPass)
            {
                CalculateUselessTables(item);
                availableTablesList.Remove(item);
            }
        }*/


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
