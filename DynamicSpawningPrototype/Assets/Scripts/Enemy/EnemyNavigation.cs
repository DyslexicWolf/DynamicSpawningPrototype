using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    public Transform navigationTarget;
    public PlayerHealth playerHealth;
    private float attackDistance = 2.5f;

    private NavMeshAgent navMeshAgent;
    private bool pathCalculate = true;
    private Vector3 startingPoint;
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
        navMeshAgent.destination = GameObject.Find("Player").transform.position;
        navigationTarget = GameObject.Find("Player").transform;
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        startingPoint = transform.position;

        //add randomization to the enemies: can add logic that only a certain amount of enemies can be fast etc.

        //this adjust the speed of the enemies
        //animator.speed = Random.Range(0.6f, 1.6f);

        //this gives some enemies the chance to push other enemies, always keep below 50 so player cant push zombies out of the way
        //navMeshAgent.avoidancePriority = Random.Range(10, 49);
    }

    private void Update()
    {
        NavMeshHit hit;
        //this checks if there is a position in front of the agent and returns true before it reaches the "maxdistance"
        //maxdistance has to be atleast 1f, otherwise it doesnt work
        //here i can set up custome navigation logic


        //if (NavMesh.SamplePosition(transform.position, out hit, 1f, 1 << NavMesh.GetAreaFromName("Walkable")))
        //{
        //    Debug.Log("on walking ground");
        //}
        //if (NavMesh.SamplePosition(transform.position, out hit, 1f, 1 << NavMesh.GetAreaFromName("Jump")))
        //{
        //    Debug.Log("on jumpground");
        //}

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
            //this means that if the target is not on an area that the agent can reach, it will go until their path stops and then return to their starting point
            //this has build in recalculation for the path if the target moves meaning that if the target gets back on an area that the agent can be, the agent starts chasing again
            if(!navMeshAgent.hasPath && pathCalculate)
            {
                navMeshAgent.destination = startingPoint;
                pathCalculate = false;
            }
            else
            {
                //animator.SetBool("Attack", false);
                navMeshAgent.destination = navigationTarget.position;
                pathCalculate = true;
            }
        }
    }

    private IEnumerator AttackDelayCoroutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackDelay);
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
