using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;  // Asignar el objeto jugador
    public Vector3 offset;    // Offset de la c�mara respecto al jugador

    void LateUpdate()
    {
        // Posiciona la c�mara en la posici�n del jugador m�s el offset
        transform.position = player.position + offset;
        transform.LookAt(player);  // Asegura que la c�mara siempre mire hacia el jugador
    }
}


