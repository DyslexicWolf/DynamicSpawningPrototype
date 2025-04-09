using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    public Transform navigationTarget;
    public PlayerHealth playerHealth;
    private float attackDistance = 2.5f;

    private NavMeshAgent navMeshAgent;
    //adjust if necessary
    private float agentSpeed = 0f;
    private Animator animator;
    private float distance;

    private int damage = 5;
    private bool canAttack = true;
    private float attackDelay = 1.5f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //uncomment when adding animations
        //animator = GetComponent<Animator>();
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

        distance = Vector3.Distance(transform.position, navigationTarget.position);
        if(distance < attackDistance && canAttack)
        {
            navMeshAgent.isStopped = true;
            //animator.SetBool("Attack", true);
            playerHealth.TakeDamage(damage);
            StartCoroutine(AttackDelayCoroutine());
        }
        else
        {
            navMeshAgent.isStopped = false;
            //animator.SetBool("Attack", false);
            navMeshAgent.destination = navigationTarget.position;
        }
    }

    private IEnumerator AttackDelayCoroutine()
    {
        Debug.Log("in attacked delay");
        canAttack = false;
        yield return new WaitForSeconds(attackDelay);
        Debug.Log("attack delay done");
        canAttack = true;
    }

    //public void OnAnimatorMove()
    //{
    //    if (animator.GetBool("Attack") == false)
    //    {
    //        //can add extra variable to increase this speed
    //        navMeshAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude + agentSpeed;
    //    }
    //}
}
