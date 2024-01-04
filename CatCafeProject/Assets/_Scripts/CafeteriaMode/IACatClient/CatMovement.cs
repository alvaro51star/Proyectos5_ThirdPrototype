using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    [SerializeField] Transform table;
    private NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        CalculateNewPath();
        Debug.Log(CalculateNewPath());
        if(CalculateNewPath())
        {
            agent.SetDestination(table.position);
        }
    }

    bool CalculateNewPath() //and check if full path is available
    {
        var path = new NavMeshPath();
        agent.CalculatePath(table.position, path);
        Debug.Log("New path calculated");
        if (agent.CalculatePath(table.position, path))
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
