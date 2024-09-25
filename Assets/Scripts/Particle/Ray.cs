using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{
    public Transform final; // El punto final del rayo
    public int cantidadDePuntos; // Número de puntos en el rayo
    public float dispersion; // La dispersión del rayo
    public float frecuencia; // Frecuencia de actualización del rayo
    public float distanciaMaxima = 10f; // Distancia máxima del rayo
    public Transform jugador; // Referencia al jugador
    public float distanciaActivacion = 5f; // Distancia para activar el ataque

    private LineRenderer line;
    private float tiempo = 0;

    void Start()
    {
        line = GetLine();
    }

    void Update()
    {
        // Calculamos la distancia entre el rayo y el jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        // Si el jugador está dentro de la distancia de activación
        if (distanciaAlJugador <= distanciaActivacion)
        {
            tiempo += Time.deltaTime;

            // Actualizamos el rayo si ha pasado el tiempo suficiente
            if (tiempo > frecuencia)
            {
                ActualizarPuntos(line);
                tiempo = 0;
            }
        }
        else
        {
            // Si el jugador no está cerca, apagamos el rayo
            line.enabled = false;
        }
    }

    private LineRenderer GetLine()
    {
        return GetComponent<LineRenderer>();
    }

    private void ActualizarPuntos(LineRenderer line)
    {
        // Actualizamos la posición del punto final multiplicado por la distancia máxima
        Vector3 puntoFinal = final.localPosition.normalized * distanciaMaxima;
        List<Vector3> puntos = InterpolarPuntos(Vector3.zero, puntoFinal, cantidadDePuntos);
        line.positionCount = puntos.Count;
        line.SetPositions(puntos.ToArray());
        line.enabled = true; // Activamos el rayo si se ha actualizado
    }

    private List<Vector3> InterpolarPuntos(Vector3 principio, Vector3 final, int totalPoints)
    {
        List<Vector3> list = new List<Vector3>();

        for (int i = 0; i < totalPoints; i++)
        {
            list.Add(Vector3.Lerp(principio, final, (float)i / totalPoints) + DesfaseAleatorio());
        }

        return list;
    }

    private Vector3 DesfaseAleatorio()
    {
        return Random.insideUnitSphere.normalized * Random.Range(0, dispersion);
    }
}
