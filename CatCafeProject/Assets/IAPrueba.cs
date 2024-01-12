using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAPrueba : MonoBehaviour
{
    public Transform objetivo;
    private NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    Vector3[] corners;
    public bool success;
    private void Update()
    {
        var path = new NavMeshPath();
       success =  NavMesh.CalculatePath(transform.position, objetivo.position, -1, path);
        corners = path.corners;

    }

    private void OnDrawGizmos()
    {
        if(corners != null)
        {
            for(int i = 0; i < corners.Length -1 ; i++)
            {
                Gizmos.DrawLine(corners[i], corners[i + 1]);
            }
        }
    }
}
