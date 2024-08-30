using UnityEngine;

public class AnimatorManager3rd : MonoBehaviour
{
    Animator animator; // Variable para almacenar el componente Animator
    int horizontal; // Variable para almacenar la versión de ID del parámetro "Horizontal" en el Animator
    int vertical; // Variable para almacenar la versión de ID del parámetro "Vertical" en el Animator

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Obtener el componente Animator

        // StringToHash convierte la cadena en un entero único llamado "hash" o "id"
        // El número "id" es único y es más eficiente, ya que es más rápido comparar enteros que cadenas
        horizontal = Animator.StringToHash("Horizontal"); // Almacenar el ID resultante en la variable
        vertical = Animator.StringToHash("Vertical"); // Almacenar el ID resultante en la variable
    }


    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isRunning)
    
    { if (isRunning)
        {
            verticalMovement = 2;
        }
        
        animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
    }
}
