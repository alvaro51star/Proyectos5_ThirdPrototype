using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    public event Action OnPathNotAvailable;

    public Transform destination;
    private NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        CalculateNewPath();
        if(CalculateNewPath())
        {
            agent.SetDestination(destination.position);
        }
        else
        {
            OnPathNotAvailable?.Invoke();//para que salga aviso de que no se puede
        }
    }

    bool CalculateNewPath() //and check if full path is available
    {
        var path = new NavMeshPath();
        agent.CalculatePath(destination.position, path);
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
