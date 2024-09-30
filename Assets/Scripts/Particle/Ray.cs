using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rayo : MonoBehaviour
{
    public Transform final; // El punto final del rayo
    public int cantidadDePuntos; // Número de puntos en el rayo
    public float dispersion; // La dispersión del rayo
    public float frecuencia; // Frecuencia de actualización del rayo
    public float distanciaMaxima = 10f; // Distancia máxima del rayo

    private LineRenderer line;
    private float tiempo = 0;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false; // Inicialmente, el rayo está desactivado
    }

    void Update()
    {
        if (line.enabled) // Solo actualiza si el rayo está activo
        {
            tiempo += Time.deltaTime;

            // Actualizamos el rayo si ha pasado el tiempo suficiente
            if (tiempo > frecuencia)
            {
                ActualizarPuntos(line);
                tiempo = 0;
            }
        }
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

    private List<Vector3> InterpolarPuntos(Vector3 principio, Vector3 final, int totalPuntos)
    {
        List<Vector3> list = new List<Vector3>();
        for (int i = 0; i < totalPuntos; i++)
        {
            list.Add(Vector3.Lerp(principio, final, (float)i / totalPuntos) + DesfaseAleatorio());
        }
        return list;
    }

    private Vector3 DesfaseAleatorio()
    {
        return Random.insideUnitSphere.normalized * Random.Range(0, dispersion);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que colisiona tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            line.enabled = true; // Activamos el rayo al entrar en contacto
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Desactivamos el rayo al salir del contacto
        if (other.CompareTag("Player"))
        {
            line.enabled = false; // Desactivamos el rayo
        }
    }
}
