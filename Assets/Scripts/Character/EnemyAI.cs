using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;  // Puntos de patrullaje
    public Transform target;  // El objetivo del enemigo (ej. el jugador)
    public float chaseRange = 10f;  // Rango de detección para perseguir
    private NavMeshAgent agent;
    private int currentPatrolIndex;
    private bool isChasing;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPatrolIndex = 0;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= chaseRange)
        {
            isChasing = true;
            agent.SetDestination(target.position);
        }
        else
        {
            isChasing = false;
            Patrol();
        }
    }

    void Patrol()
    {
        if (agent.remainingDistance < agent.stoppingDistance && !isChasing)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }
}


