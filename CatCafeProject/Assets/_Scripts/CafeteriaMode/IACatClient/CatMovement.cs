using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    public event Action OnPathNotAvailable;

    [SerializeField] private CatDataSO catData;
    public Transform initialDestination;
    public TablesManager tablesManager;
    private NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        MovementToDestination(initialDestination);

    }

    public void MovementToDestination(Transform destination)
    {
        if (CalculateNewPath(destination))
        {
            agent.SetDestination(destination.position);
        }
        else
        {
            OnPathNotAvailable?.Invoke();//para que salga aviso de que no se puede
        }        
    }

    public bool CalculateNewPath(Transform destination) //and check if full path is available
    {
        var path = new NavMeshPath();

        if (agent.CalculatePath(destination.position, path))
        {
            if (path.status != NavMeshPathStatus.PathComplete)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }        
    }

    /*public IEnumerator WaitForClientMovement()
     {
         TableData tableAssigned = CheckAvailableTables();
         tableAssigned.TableIsFree();
         tableAssigned.isOcupied = true;

         yield return new WaitForSeconds(2f);

         Transform destination = tableAssigned.selectedChair;
         MovementToDestination(destination);
         Debug.Log("client moves to  " + destination);
     }

    public TableData CheckAvailableTables()//en futuro llamado por evento en TableData
    {
        List<TableData> availableTableDataList = new List<TableData>();

        for (int i = 0; i < tablesManager.tableDataList.Count; i++)
        {
            if (!tablesManager.tableDataList[i].isOcupied) //&& CalculateNewPath(availableTableDataList[i].selectedChair)
            {
                availableTableDataList.Add(tablesManager.tableDataList[i]);
            }            
        }

        for (int i = 0; i < catData.likes.Count; i++)
        {
            for (int j = 0; j < availableTableDataList.Count; j++)
            {
                if (catData.likes[i] == availableTableDataList[j].furnitureTheme)
                {
                    return availableTableDataList[j];
                }
            }
        }      

        /*for(int i = 0; i < availableTableDataList.Count; i++) //como hago que return la que menos distancia???
        {
            Vector3.Distance(this.gameObject.transform.position, availableTableDataList[i].selectedChair.position);
        }

        return null;
    }*/
}
