using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class COntrollerVida : MonoBehaviour
{
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMaxLife(float vidaMaxima){
        slider.maxValue = vidaMaxima;
    }

    public void ChangeLife(float cantidadVida){
        slider.value = cantidadVida;
    }

    public void StartLife(float cantidadVida){
        ChangeMaxLife(cantidadVida);
        ChangeLife(cantidadVida);
    }
}
