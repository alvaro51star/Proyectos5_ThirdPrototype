using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    public event Action OnPathNotAvailable;

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

    private bool CalculateNewPath(Transform destination) //and check if full path is available
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
}
