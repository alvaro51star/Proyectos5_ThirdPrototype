using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    public Transform initialDestination;
    public TablesManager tablesManager;
    [SerializeField] private NavMeshAgent agent;
    public TableData tableAssigned;
    private void Start()
    {
 
        MovementToDestination(initialDestination);

    }

    public void MovementToDestination(Transform destination)
    {
        agent.SetDestination(destination.position);
    }

    public bool CalculateNewPath(Transform destination) //and check if full path is available
    {        
        var path = new NavMeshPath();

        bool success = NavMesh.CalculatePath(agent.transform.position, destination.position, -1, path);

        if(success)
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

    public IEnumerator WaitForClientMovement()
     {
        tableAssigned = tablesManager.CheckAvailableTables();                 

        yield return new WaitForSeconds(2f);

        if (tableAssigned)
        {
            tableAssigned.ResetTableData(false);//to get selectedChair
            Transform destination = tableAssigned.selectedChair;

            MovementToDestination(destination);
        }

        else
        {
            Debug.Log("no hay mesas libres");
        }
    }    
}
