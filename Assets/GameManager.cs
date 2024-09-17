using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Referencias a los scripts del jugador
    public PlayerManager3rd playerManager;
    public PlayerMovement3rd playerMovement;
    public InputManager3rd inputManager;
    public AnimatorManager3rd animatorManager;
    public PlayerControls3rd playerControls;

    // Referencia al script del enemigo
    public EnemyAI enemyAI;

    // Distancia de interacción entre el enemigo y el jugador
    public float interactionDistance = 5f;

    void Start()
    {
        // Comprobamos que todas las referencias estén asignadas correctamente
        if (playerManager == null || playerMovement == null || inputManager == null ||
            animatorManager == null || playerControls == null || enemyAI == null)
        {
           /* Debug.LogError("Faltan referencias en el GameManager");*/
        }

        // Inicializar los sistemas de jugador y enemigo
        InitializeGame();
    }

    void Update()
    {
        // Detectar interacciones entre el enemigo y el jugador
        CheckEnemyPlayerInteraction();
    }

    void InitializeGame()
    {
        // Inicializar cualquier comportamiento o estado inicial si fuera necesario
        Debug.Log("Juego Inicializado");
    }

    void CheckEnemyPlayerInteraction()
    {
        // Detectar si el enemigo está lo suficientemente cerca del jugador
        float distance = Vector3.Distance(playerManager.transform.position, enemyAI.transform.position);

        if (distance < interactionDistance)
        {
            Debug.Log("El enemigo está cerca del jugador.");
            enemyAI.agent.SetDestination(playerManager.transform.position); // Seguir al jugador
        }
    }
}
