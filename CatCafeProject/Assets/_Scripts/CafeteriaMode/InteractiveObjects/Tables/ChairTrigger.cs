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

            if (other.GetComponent<CatMovement>().tableAssigned == tableData)
            {
                NavMeshAgent agent = other.GetComponent<NavMeshAgent>();

                if (other.GetComponent<ClientStates>().catState != CatState.Leaving)
                {
                    agent.isStopped = true;

                    if (agent.isStopped)
                    {
                        other.transform.rotation = transform.rotation;
                        other.GetComponent<CatMovement>().m_sit = true;
                    }
                }
                else
                {
                    agent.isStopped = false;
                    other.GetComponent<CatMovement>().m_sit = false;
                    other.GetComponent<CatMovement>().m_eating = false;
                }
            }
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<CatMovement>() && other.GetComponent<CatMovement>().tableAssigned == tableData)
        {
            tableData.ResetTableData(false);
        }
    }
}
