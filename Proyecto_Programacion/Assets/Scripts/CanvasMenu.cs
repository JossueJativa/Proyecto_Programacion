using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMenu : MonoBehaviour
{
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject menuPausa;
    private bool pause = false;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(pause){
                Reanudar();
            }else{
                Pausa();
            }
        }
    }
    // Start is called before the first frame update
    public void Pausa(){
        pause = true;
        Time.timeScale = 0;
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
    }

    public void Reanudar(){
        pause = false;
        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
    }

    public void exit(){
        Application.Quit();
        Debug.Log("Salir");
    }
}
