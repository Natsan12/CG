using UnityEngine;

public class AnimatorManager3rd : MonoBehaviour
{
    Animator animator; // Variable para almacenar el componente Animator
    int horizontal; // Variable para almacenar la versi�n de ID del par�metro "Horizontal" en el Animator
    int vertical; // Variable para almacenar la versi�n de ID del par�metro "Vertical" en el Animator
    int jump; // Para el salto
    int kickAttack; // Para el ataque de patada
    int swordAttack; // Para el ataque de espada

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Obtener el componente Animator

        // StringToHash convierte la cadena en un entero �nico llamado "hash" o "id"
        // El n�mero "id" es �nico y es m�s eficiente, ya que es m�s r�pido comparar enteros que cadenas
        horizontal = Animator.StringToHash("Horizontal"); // Almacenar el ID resultante en la variable
        vertical = Animator.StringToHash("Vertical"); // Almacenar el ID resultante en la variable
        jump = Animator.StringToHash("Jump"); // Asignar el par�metro del salto
        kickAttack = Animator.StringToHash("KickAttack"); // Asignar el par�metro de patada
        swordAttack = Animator.StringToHash("SwordAttack"); // Asignar el par�metro de espada
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isRunning)
    {
        if (isRunning)
        {
            verticalMovement = 2;
        }

        animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        animator.applyRootMotion = false;
        animator.SetTrigger(targetAnimation);
        Debug.Log("Triggering animation: " + targetAnimation);
    }
}
