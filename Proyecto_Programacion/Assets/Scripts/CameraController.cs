using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform objetivo; // Aquí arrastra el objeto del personaje que quieres seguir
    public Vector3 offset = new Vector3(0, 0, -10); // Ajusta la posición de la cámara en relación al personaje


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        potitionCamera();
    }

    void potitionCamera(){
        if (objetivo != null)
        {
            // Obtén la posición actual del personaje
            Vector3 posicionObjetivo = objetivo.position + offset;

            // Interpola suavemente la posición de la cámara hacia la posición del personaje
            transform.position = Vector3.Lerp(transform.position, posicionObjetivo, Time.deltaTime * 5f);
        }
    }
}
