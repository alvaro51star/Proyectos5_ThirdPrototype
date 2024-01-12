using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    //[SerializeField] private CatDataSO catData;
    public Transform initialDestination;
    public TablesManager tablesManager;
    [SerializeField] private NavMeshAgent agent;
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
        Debug.Log(path.status+" "+destination.name);//DA PATH INVALID PORQUE AL HABER LOS AGUJEROS DE LAS MESAS NO ENCUENTRA PATH

        //if (agent.CalculatePath(destination.position, path))
        if(success)
        {            
            /*if (path.status != NavMeshPathStatus.PathComplete)
            {
                return false;
            }
            else
            {
                return true;
            }*/
            return true;
        }
        else
        {
            Debug.Log("no puede pasar ahi");
            return false;
        }
    }

    public IEnumerator WaitForClientMovement()
     {
         TableData tableAssigned = tablesManager.CheckAvailableTables();//devuelve null                 

        yield return new WaitForSeconds(2f);

        if (tableAssigned)
        {
            tableAssigned.ResetTableData(false);//to get selectedChair
            Transform destination = tableAssigned.selectedChair;
            Debug.Log("destino" + destination);

            MovementToDestination(destination);
            Debug.Log("client moves to  " + destination);
        }

        else
        {
            Debug.Log("tableAssigned = " + tableAssigned);
        }
    }    
}
