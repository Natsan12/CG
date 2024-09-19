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

    public float detectionRange = 10f; // Distancia de detecci�n del jugador
    public float attackRange = 2f; // Distancia de ataque
    private int currentPatrolIndex;

    // Variables para optimizaci�n de animaciones
    private bool isWalking = false;
    private bool isIdle = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Iniciar el patrullaje si hay puntos disponibles y el agente est� en la NavMesh
        if (patrolPoints.Length > 0 && agent.isOnNavMesh)
        {
            MoveToNextPatrolPoint();
        }
    }

    void Update()
    {
        // Calcular la distancia al jugador una sola vez y en base a distancias al cuadrado para optimización
        float sqrDistanceToPlayer = (player.position - transform.position).sqrMagnitude;

        if (sqrDistanceToPlayer <= attackRange * attackRange)
        {
            AttackPlayer();
        }
        else if (sqrDistanceToPlayer <= detectionRange * detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            // Reanudar el movimiento del agente si estaba detenido
            if (agent.isStopped)
            {
                agent.isStopped = false;
            }

            Patrol();
        }
    }

    // Metodo para mover al enemigo a la siguiente posicion de patrullaje
    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        MoveToPosition(patrolPoints[currentPatrolIndex].position);
    }

    // Metodo para mover al enemigo a una posici�n espec�fica
    void MoveToPosition(Vector3 targetPosition)
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(targetPosition);
        }
        else
        {
            Debug.LogWarning("El agente no est� en la NavMesh.");
        }
    }

    // Metodo para patrullar entre los puntos
    void Patrol()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            MoveToNextPatrolPoint();
        }

        UpdateAnimations(true); // Activar animaci�n de caminar
    }

    // M�todo para perseguir al jugador
    void ChasePlayer()
    {
        MoveToPosition(player.position);
        UpdateAnimations(true); // Activar animaci�n de caminar
    }

    // M�todo para atacar al jugador
    void AttackPlayer()
    {
        agent.isStopped = true; // Detener el movimiento
        Vector3 directionToPlayer = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToPlayer); // Girar hacia el jugador

        UpdateAnimations(false); // Desactivar animaci�n de caminar
        animator.SetTrigger("Attack"); // Activar animaci�n de ataque
    }

    // M�todo para cambiar las animaciones de caminar e idle seg�n el estado actual
    void UpdateAnimations(bool walking)
    {
        SetAnimationState(walking, !walking); // Cambiar entre caminar e idle
    }

    // M�todo que cambia el estado de animaciones si es necesario (evita llamadas repetitivas)
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
