using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChairTrigger : MonoBehaviour
{
    [SerializeField] private TableData tableData;
   
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<ClientStates>())
        {
            other.GetComponent<ClientStates>().enabled = true;

            if (other.GetComponent<CatMovement>().tableAssigned = tableData)
            {
                NavMeshAgent agent = other.GetComponent<NavMeshAgent>();

                if (other.GetComponent<ClientStates>().catState != CatState.Leaving)
                {
                    agent.isStopped = true;

                    if (agent.isStopped)
                    {
                        other.transform.rotation = transform.rotation;
                    }
                }
                else
                {
                    agent.isStopped = false;
                    tableData.isOccupied = false;
                }
            }
        }       
    }
}
