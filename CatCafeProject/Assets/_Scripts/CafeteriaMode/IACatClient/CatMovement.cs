using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    //public Transform initialDestination;
    public TablesManager tablesManager;
    [SerializeField] private NavMeshAgent agent;
    public TableData tableAssigned;

    public static event Action OnTableAssigned;

    private void OnEnable()
    {
        agent.enabled = true;
    }

    public void MovementToDestination(Transform destination)
    {
        if(CalculateNewPath(destination))
        {
            agent.SetDestination(destination.position);            
        }
        else
        {
            Debug.Log("no puede pasar a esa mesa");
        }
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

    public IEnumerator WaitForMovementToAssignedTable()
    {
        Debug.Log(name + "waitForMovmentToAssignedTable");
        if(AssignTable())
        {            
            yield return new WaitForSeconds(2f);

            Transform destination = tableAssigned.selectedChair;
            MovementToDestination(destination);
            
            OnTableAssigned?.Invoke();

        }
        else
        {
            StartCoroutine(WaitForMovementToAssignedTable());
            Debug.Log("esperando una mesa libre");
        }
    }    

    private bool AssignTable()
    {
        tableAssigned = tablesManager.CheckAvailableTables();

        if (tableAssigned != null)
        {
            Debug.Log($"La mesa es: {tableAssigned}");

            return true;
        }

        else
        {
            Debug.Log("no hay mesas libres");

            return false;
        }
    }
}
