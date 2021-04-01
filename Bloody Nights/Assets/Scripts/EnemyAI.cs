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

    [SerializeField] private float rotation_Speed = 5.0f;

    private Animator enemy_Animator = null;

    private bool is_Provoked = false;

    void Awake()
    {
        enemy_Animator = GetComponent<Animator>();
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
        if (distance_ToTarget > nav_M_Agent.stoppingDistance)
        {
            enemy_Animator.SetBool("Attack", false);
            enemy_Animator.SetTrigger("Move");
            ChaseTarget();
        }
        else if(distance_ToTarget <= nav_M_Agent.stoppingDistance + 0.1f)
        {
            AttackTarget();
            RotateTowardsTraget();
        }
    }

    private void ChaseTarget()
    {

        nav_M_Agent.SetDestination(Target.position);
    }

    private void AttackTarget()
    {
        enemy_Animator.SetBool("Attack", true);
    }

    private void RotateTowardsTraget()
    {
        Vector3 relative_TargetPosition = (Target.position - transform.position).normalized;

        Quaternion lookAt_Roation = Quaternion.LookRotation(new Vector3(relative_TargetPosition.x , 0.0f , relative_TargetPosition.z));

        transform.rotation = Quaternion.Slerp(transform.rotation , lookAt_Roation , rotation_Speed * Time.deltaTime);
    }

    public void OnDamageTaken()
    {
        is_Provoked = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chace_Range);
    }
}
