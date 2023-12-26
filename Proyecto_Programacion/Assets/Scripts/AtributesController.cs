using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtributesController : MonoBehaviour
{
    [SerializeField] private int cant;    

    public int getCant(){
        return this.cant;
    }

    public void setCant(int cant){
        this.cant = cant;
    }
}
