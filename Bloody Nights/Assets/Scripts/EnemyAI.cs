using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform Target = null;
    [SerializeField] float chace_Range = 5.0f;
    private float distance_ToTarget = Mathf.Infinity;
    NavMeshAgent nav_M_Agent = null;

    private bool is_Provoked = false;

    void Awake()
    {
        nav_M_Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        SearchAndDestroy();
    }

    private void SearchAndDestroy()
    {
        distance_ToTarget = Vector3.Distance(transform.position, Target.position);
        if(is_Provoked)
        {
            EngageTarget();
        }
        else if (distance_ToTarget <= chace_Range)
        {
            is_Provoked = true;
            //nav_M_Agent.SetDestination(Target.position);
        }
    }

    private void EngageTarget()
    {
        if (distance_ToTarget >= nav_M_Agent.stoppingDistance)
        {
            ChaseTarget();
        }
        else
            AttackTarget();
    }

    private void ChaseTarget()
    {
        nav_M_Agent.SetDestination(Target.position);
    }

    private void AttackTarget()
    {
        print("Attacking " + Target.name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chace_Range);
    }
}
