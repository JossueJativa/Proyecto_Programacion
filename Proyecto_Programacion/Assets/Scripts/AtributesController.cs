using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtributesController : MonoBehaviour
{
    [SerializeField] private int cant;
    [SerializeField] private String type;
    [SerializeField] private String subtype;
    private ConstructionController constructionController;
    private PlayerController playerController;

    private void Start()
    {
        constructionController = GameObject.FindObjectOfType<ConstructionController>();
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    public int getCant(){
        return this.cant;
    }

    public void setCant(int cant){
        this.cant = cant;
    }

    public String getType(){
        return this.type;
    }

    public String getSubtype(){
        return this.subtype;
    }
    public void Action(){
        if(type == "block"){
            constructionController.activateBlock();
            playerController.deactivateAttack();
            if (subtype == "magma"){
                Debug.Log("Magma");
            }
            else if(type == "dirt"){
                Debug.Log("Dirt");
            }
        }
        else if(type == "Potion"){
            constructionController.deactivateBlock();
            playerController.deactivateAttack();
            if(subtype == "life"){
                Debug.Log("Life");    
            }
            else if(subtype == "mana"){
                Debug.Log("Mana");
            }
        }
        else if(type == "Weapon"){
            constructionController.deactivateBlock();
            playerController.activateAttack();
            if(subtype == "sword"){
                Debug.Log("Sword");
            }
            else if(subtype == "bow"){
                Debug.Log("Bow");
            }
        }
    }
}
