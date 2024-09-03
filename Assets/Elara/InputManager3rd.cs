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
    public bool jumpInput; // Para el salto
    public bool kickInput; // Cambiado de KickAttack para consistencia
    public bool swordInput; // Para el ataque de espada

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

            playerControls.PlayerActions.Space.performed += i => jumpInput = true; // Input de salto
            //playerControls.PlayerActions.Space.canceled += i => jumpInput = false;

            playerControls.PlayerActions.LeftButton.performed += i => kickInput = true; // Input de patada, cambiado de KickAttack
            //playerControls.PlayerActions.LeftButton.canceled += i => kickInput = false;

            playerControls.PlayerActions.RigthtButton.performed += i => swordInput = true; // Input de espada
           playerControls.PlayerActions.LeftButton.canceled += i => kickInput = false;
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
        // Llamamos a otros m�todos de manejo de inputs
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x; // Correcci�n del nombre de la variable
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerMovement.isRunning);
    }

    public void HandleAllInputs() // Cambiado el nombre del m�todo para corregir el typo
    {
        HandleMovementInput();
        HandleRunningInput();
        HandleActionInputs();
    }

    private void HandleActionInputs()
    {
        if (jumpInput)
        {
            animatorManager.PlayTargetAnimation("Jump", false); // Ejecutar la animaci�n de salto
            jumpInput = false;
        }

        if (kickInput) // Cambiado de KickAttack
        {
            animatorManager.PlayTargetAnimation("KickAttack", true); // Ejecutar la animaci�n de patada
            kickInput = false;
        } 

        if (swordInput)
        {
            animatorManager.PlayTargetAnimation("SwordAttack", true); // Ejecutar la animaci�n de espada
            swordInput = false;
        }
    }

    private void HandleRunningInput() // Cambiado el nombre del m�todo para corregir el typo
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
