using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    public Transform navigationTarget;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = navigationTarget.position;
    }

    private void Update()
    {
        NavMeshHit hit;
        //this checks if there is a position in front of the agent and returns true before it reaches the "maxdistance"
        //maxdistance has to be atleast 1f, otherwise it doesnt work
        //here i can set up custome navigation logic
        if (NavMesh.SamplePosition(transform.position, out hit, 1f, 1 << NavMesh.GetAreaFromName("Walkable")))
        {
            Debug.Log("on walking ground");
        }
        if (NavMesh.SamplePosition(transform.position, out hit, 1f, 1 << NavMesh.GetAreaFromName("Jump")))
        {
            Debug.Log("on jumpground");
        }

    }
}
