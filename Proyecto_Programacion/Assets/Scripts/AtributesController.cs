using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtributesController : MonoBehaviour
{
    [SerializeField] private int cant;
    [SerializeField] private String type;
    [SerializeField] private String subtype;

    public int getCant(){
        return this.cant;
    }

    public void setCant(int cant){
        this.cant = cant;
    }

    public void Action(){
        if(type == "block"){
            if (subtype == "magma"){
                Debug.Log("Magma");
            }
            else if(type == "dirt"){
                Debug.Log("Dirt");
            }
        }
        else if(type == "Potion"){
            if(subtype == "life"){
                Debug.Log("Life");
            }
            else if(subtype == "mana"){
                Debug.Log("Mana");
            }
        }
        else if(type == "Weapon"){
            if(subtype == "sword"){
                Debug.Log("Sword");
            }
            else if(subtype == "bow"){
                Debug.Log("Bow");
            }
        }
    }
}
