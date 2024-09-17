using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints; // Los puntos de patrullaje
    public Transform player; // El jugador
    public NavMeshAgent agent;
    private Animator animator;

    public float detectionRange = 10f; // Distancia de detección del jugador
    public float attackRange = 2f; // Distancia de ataque
    private int currentPatrolIndex;

    // Variables para optimización de animaciones
    private bool isWalking = false;
    private bool isIdle = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Iniciar el patrullaje si hay puntos disponibles y el agente está en la NavMesh
        if (patrolPoints.Length > 0 && agent.isOnNavMesh)
        {
            MoveToNextPatrolPoint();
        }
    }

    void Update()
    {
        // Optimización: Calcular la distancia al jugador una sola vez y en base a distancias al cuadrado
        float sqrDistanceToPlayer = (player.position - transform.position).sqrMagnitude;
        float sqrDetectionRange = detectionRange * detectionRange;
        float sqrAttackRange = attackRange * attackRange;

        if (sqrDistanceToPlayer <= sqrAttackRange)
        {
            AttackPlayer();
        }
        else if (sqrDistanceToPlayer <= sqrDetectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    // Método para mover al enemigo a la siguiente posición de patrullaje
    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        MoveToPosition(patrolPoints[currentPatrolIndex].position);
    }

    // Método para mover al enemigo a una posición específica
    void MoveToPosition(Vector3 targetPosition)
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(targetPosition);
        }
        else
        {
            Debug.LogWarning("El agente no está en la NavMesh.");
        }
    }

    // Método para patrullar entre los puntos
    void Patrol()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            MoveToNextPatrolPoint();
        }

        UpdateAnimations(true); // Activar animación de caminar
    }

    // Método para perseguir al jugador
    void ChasePlayer()
    {
        MoveToPosition(player.position);
        UpdateAnimations(true); // Activar animación de caminar
    }

    // Método para atacar al jugador
    void AttackPlayer()
    {
        agent.isStopped = true; // Detener el movimiento
        Vector3 directionToPlayer = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToPlayer); // Girar hacia el jugador

        UpdateAnimations(false); // Desactivar animación de caminar
        animator.SetTrigger("Attack"); // Activar animación de ataque
    }

    // Método para cambiar las animaciones de caminar e idle según el estado actual
    void UpdateAnimations(bool walking)
    {
        SetAnimationState(walking, !walking); // Cambiar entre caminar e idle
    }

    // Método que cambia el estado de animaciones si es necesario (evita llamadas repetitivas)
    void SetAnimationState(bool walking, bool idle)
    {
        if (isWalking != walking)
        {
            animator.SetBool("isWalking", walking);
            isWalking = walking;
        }

        if (isIdle != idle)
        {
            animator.SetBool("isIdle", idle);
            isIdle = idle;
        }
    }
}
