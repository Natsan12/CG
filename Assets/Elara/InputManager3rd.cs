using UnityEngine;

public class InputManager3rd : MonoBehaviour
{
    PlayerControls3rd playerControls;
    AnimatorManager3rd animatorManager;
    PlayerMovement3rd playerMovement;

    public Vector2 movementInput;
    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool shiftInput;
    //public object Public { get; private set; }
    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager3rd>();
        playerMovement = GetComponent<PlayerMovement3rd>();
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls3rd();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Shift.performed += i => shiftInput = true;
            playerControls.PlayerActions.Shift.canceled += i => shiftInput = false;



        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Propiedades para acceder a los inputs de movimiento vertical y horizontal
    private void HandleMovementInput()
    {
        // Llamamos a otros métodos de manejo de inputs
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x; // Corrección del nombre de la variable
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
     animatorManager.UpdateAnimatorValues(0, moveAmount, playerMovement.isRunning);
       
    }
    public void handleAllInpunts()
    {
        HandleMovementInput();
        HadleisRunningInput();

    }

    private void HadleisRunningInput()

    {
        if (shiftInput && moveAmount > 0.5f)

        {
            playerMovement.isRunning = true;
        }

        else
        {
            playerMovement.isRunning = false;
        }

    }
}


