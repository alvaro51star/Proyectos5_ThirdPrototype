using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    public TablesManager tablesManager;
    [SerializeField] private NavMeshAgent agent;
    public TableData tableAssigned;

    public static event Action OnTableAssigned;

    private Animator cmpCatAnimator;
    public bool m_sit;
    public bool m_eating;

    private void Start()
    {
        cmpCatAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        cmpCatAnimator.SetFloat("Speed", agent.velocity.magnitude);
        SitAnimation();
        EatingAnimation();
    }

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
            //Debug.Log($"La mesa es: {tableAssigned}");

            return true;
        }

        else
        {
            Debug.Log("no hay mesas libres");

            return false;
        }
    }

    private void SitAnimation()
    {
        if (m_sit == true)
        {
            Debug.Log("sentado");
            cmpCatAnimator.SetBool("Sit", true);
            cmpCatAnimator.SetBool("Eat", false);
        }
        else
        {
            cmpCatAnimator.SetBool("Sit", false);
            cmpCatAnimator.SetBool("Eat", true);
        }
    }

    private void EatingAnimation()
    {
        if (m_eating == true)
        {
            Debug.Log("comiendo");
            cmpCatAnimator.SetBool("Eat", true);
            cmpCatAnimator.SetBool("Sit", false);
        }
        else
        {
            cmpCatAnimator.SetBool("Eat", false);
            cmpCatAnimator.SetBool("Sit", true);
        }
    }
}
