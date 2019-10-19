using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 10f;
    [SerializeField] float turnSpeed = 3f;

    private NavMeshAgent navMeshAgent;
    private float distanceToTarget = Mathf.Infinity;
    private bool isProvoked = false;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked){
            EngageTarget();
        } else if (distanceToTarget < chaseRange){
            isProvoked = true;
        }

    }

    public void OnDamageTaken(){
        isProvoked = true;
    }

    private void EngageTarget(){

        FaceTarget();
        if (distanceToTarget > navMeshAgent.stoppingDistance){
            ChaseTarget();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance){
            AttackTarget();
        }
    }

    private void ChaseTarget(){
        GetComponent<Animator>().SetBool("Attack", false);
        GetComponent<Animator>().SetBool("Walk", true);
        navMeshAgent.destination = target.position;
    }

    private void FaceTarget(){
        // transform.rotation = where the target is need to rotate certain speed.
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void AttackTarget(){
        GetComponent<Animator>().SetBool("Walk", false);
        GetComponent<Animator>().SetBool("Attack", true);
        Debug.Log("Attack!");
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
