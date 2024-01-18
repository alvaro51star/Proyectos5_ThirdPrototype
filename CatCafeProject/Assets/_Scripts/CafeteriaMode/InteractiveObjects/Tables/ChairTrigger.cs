using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChairTrigger : MonoBehaviour
{
    [SerializeField] private TableData tableData;

    private void OnTriggerStay(Collider other)
    {

            if (other.GetComponent<CatMovement>().tableAssigned = tableData)
            {
                NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
                agent.isStopped = true;
                if (agent.remainingDistance == 0f)
                {
                    other.transform.rotation = this.transform.rotation;
                }
            }
    }
}
