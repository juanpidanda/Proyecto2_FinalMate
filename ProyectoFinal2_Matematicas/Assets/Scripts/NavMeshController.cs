using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        GetAgent();
    }
    void GetAgent()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void SetAgentSpeed(float speedToSet)
    {
        navMeshAgent.speed = speedToSet;
    }

    public void SetNextAgentObjective(GameObject objective)
    {
        navMeshAgent.destination = objective.transform.position;
        navMeshAgent.isStopped = false;
    }
    public void StopAgent()
    {
        navMeshAgent.isStopped = true;
    }


    public bool HasArrived()
    {
        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
