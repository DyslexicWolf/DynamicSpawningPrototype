using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Unity.AI.Navigation.Samples
{
    public enum OffMeshLinkMoveMethod
    {
        Teleport,
        NormalSpeed,
        Parabola,
        Curve
    }

    /// <summary>
    /// Move an agent when traversing a OffMeshLink given specific animated methods
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentLinkMover : MonoBehaviour
    {
        public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola;
        public AnimationCurve animationCurve = new AnimationCurve();
        //private Animator animator;
        private NavMeshAgent navMeshAgent;

        private void Awake()
        {
            //animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        IEnumerator Start()
        {
            navMeshAgent.autoTraverseOffMeshLink = false;
            while (true)
            {
                if (navMeshAgent.isOnOffMeshLink)
                {
                    if (method == OffMeshLinkMoveMethod.NormalSpeed)
                        yield return StartCoroutine(NormalSpeed(navMeshAgent));
                    else if (method == OffMeshLinkMoveMethod.Parabola)
                        yield return StartCoroutine(Parabola(navMeshAgent, 2.0f, 0.5f));
                    else if (method == OffMeshLinkMoveMethod.Curve)
                        yield return StartCoroutine(Curve(navMeshAgent, 0.5f));
                    navMeshAgent.CompleteOffMeshLink();
                }

                yield return null;
            }
        }

        IEnumerator NormalSpeed(NavMeshAgent agent)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            while (agent.transform.position != endPos)
            {
                agent.transform.position =
                    Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
                yield return null;
            }
        }

        //for the standard parabola curve, adjust height, etc, in the start method
        IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            float normalizedTime = 0.0f;
            while (normalizedTime < 1.0f)
            {
                //if(endPos.y > startPos.y)
                //{
                //    //Debug.Log("is jumping up");
                //    //animator.SetBool("isJumping", true);
                //}
                //if(endPos.y < startPos.y)
                //{
                //    //Debug.Log("is dropping down");
                //    //animator.SetBool("isDroppingDown", true);
                //}
                float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }

        //for if you want a custome curve, like jumping up to a ledge, ...
        IEnumerator Curve(NavMeshAgent agent, float duration)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            float normalizedTime = 0.0f;
            while (normalizedTime < 1.0f)
            {
                //if(endPos.y > startPos.y)
                //{
                //    //Debug.Log("is jumping up");
                //    //animator.SetBool("isJumping", true);
                //}
                //if(endPos.y < startPos.y)
                //{
                //    //Debug.Log("is dropping down");
                //    //animator.SetBool("isDroppingDown", true);
                //}
                float yOffset = animationCurve.Evaluate(normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }

        private void Update()
        {
            if(navMeshAgent.isOnOffMeshLink == false)
            {
                //Debug.Log("walking");
                //animator.SetBool("isJumping", false);
            }
        }
    }
}