using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemy : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;
    public float attackRadius;
    public float talkRadius;
    public float forgetTime;
    public float turnSpeed = 5f;
    
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public bool isSearching;
    public bool isChasing;
    public bool isAttacking;

    public List<Transform> visibleTargets = new List<Transform>();
    public List<NewTransform> lastSeenTargets = new List<NewTransform>();

    private NavMeshAgent navMeshAgent;
    
    private float lastTimeSeen;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine ("FindTargetsWithDelay", .2f);
    }

    void Patrol()
    {
        // Move Anywhere
        navMeshAgent.destination = transform.position;
        Debug.Log("Patrolling");
    }

    void Search()
    {
        // Move Towards Last Seen Target
        navMeshAgent.destination = lastSeenTargets[0].position;
        FaceTarget(lastSeenTargets[0].position);
        Debug.Log("Searching");
    }
    
    void Chase()
    {
        // Move Towards Visible Target
        navMeshAgent.destination = visibleTargets[0].position;
        FaceTarget(visibleTargets[0].position);
        Debug.Log("Chasing");
    }
    
    void Update()
    {
        if (Time.time >= (lastTimeSeen + forgetTime))
        {
            isSearching = false;
            lastSeenTargets.Clear();
        }
        else
        {
            isSearching = true;
        }

        if (!isAttacking)
        {
            if (isChasing)
            {
                Chase();
            }else if (isSearching)
            {
                Search();
            }
            else
            {
                Patrol();
            }
        }
        else
        {
            navMeshAgent.destination = transform.position;
        }
    }
    
    IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds (delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets() {
        visibleTargets.Clear ();
        isChasing = false;
        isAttacking = false;
        Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius [i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) {
                float dstToTarget = Vector3.Distance (transform.position, target.position);
                if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    if (Physics.CheckSphere(transform.position, attackRadius, targetMask))
                    {
                        lastTimeSeen = float.NegativeInfinity;
                        isAttacking = true;
                        lastSeenTargets.Clear();
                    }
                    else
                    {
                        lastTimeSeen = Time.time;
                        isChasing = true;
                    }
                    visibleTargets.Add(target);
                    SearchTransformID(target);
                }
            }
        }
    }

    void SearchTransformID(Transform target)
    {
        for (int i = 0; i < lastSeenTargets.Count; i++)
        {
            if (lastSeenTargets[i].id == target.GetInstanceID())
            {
                return;
            }
        }

        NewTransform newTransform = new NewTransform();
        newTransform.position = target.position;
        newTransform.id = target.GetInstanceID();
        
        lastSeenTargets.Add(newTransform);
    }

    void FaceTarget(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}

public struct NewTransform
{
    public NewTransform(Vector3 _position, float _id)
    {
        position = _position;
        id = _id;
    }

    public Vector3 position { get; set; }
    public float id { get; set; }
}