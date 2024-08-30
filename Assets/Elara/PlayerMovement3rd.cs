using UnityEngine;

public class PlayerMovement3rd : MonoBehaviour
{
    InputManager3rd inputManager; // para referenciar el script InputManager3rd
    Vector3 moveDirection; // para almacenar la dirección del movimiento
    Transform cameraObject; // para referenciar la cámara
    Rigidbody playerRigidbody;
    public float walkingSpeed = 2.5f;
    public float runningSpeed = 7;
    //public float jumpingSpeed = 10;
    //public float attackedKickSpeed = 1;
    //public float attackedSwordSpeed = 1;

    public float rotationSpeed = 15; // para controlar la velocidad de rotación

    public bool isRunning   ;
    private void Awake()
    {
        inputManager = GetComponent<InputManager3rd>(); // obtener el componente InputManager3rd
        playerRigidbody = GetComponent<Rigidbody>(); // obtener el componente Rigidbody
        cameraObject = Camera.main.transform; // buscar la cámara principal en la escena
    }

    // Update es llamado una vez por frame
    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;

        // Agregar el input horizontal a la dirección de movimiento. A (izquierda) y D (derecha)
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput; // Corrección del nombre de la variable

        moveDirection.y = 0; // para que el jugador no pueda moverse hacia el cielo

        // Mantener el vector en la misma dirección pero cambiar su magnitud a 1 para mantener la misma aceleración
        // en todas las direcciones, incluso en diagonales.
        moveDirection.Normalize();


        if (isRunning)

        {
            moveDirection = moveDirection * runningSpeed;
        }
        else


        {
            moveDirection = moveDirection * walkingSpeed;

        }

        // para controlar la velocidad de movimiento desde el editor

        // almacenar la dirección final y la velocidad del movimiento en una nueva variable movementVelocity
        Vector3 movementVelocity = moveDirection;

        // aplicar el cálculo de velocidad previo a la velocidad del rigidbody
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation() //"la idea es enfrentar primero la dirección a la que quieres moverte, y luego moverte"
    {
        Vector3 targetDirection = Vector3.zero; // una nueva variable para almacenar la rotación a la que se girará el jugador

        targetDirection = cameraObject.forward * inputManager.verticalInput;

        // Agregar el input horizontal a la dirección de movimiento. A (izquierda) y D (derecha)
        targetDirection += cameraObject.right * inputManager.horizontalInput; // Corrección del nombre de la variable

        targetDirection.y = 0; // para que el jugador no pueda moverse hacia el cielo

        // Mantener el vector en la misma dirección pero cambiar su magnitud a 1 para
        // mantener la misma aceleración en todas las direcciones, incluso en diagonales.
        targetDirection.Normalize();

        // Para mirar hacia nuestra rotación objetivo
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Realizar la rotación entre la orientación actual y la rotación objetivo usando una velocidad de rotación independiente de frame
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation; // aplicar la rotación final al jugador
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }
}
