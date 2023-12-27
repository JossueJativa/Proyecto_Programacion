using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private int cant;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            GameObject[] inventory = GameObject.FindGameObjectWithTag("GenerateEvent").GetComponent<InventoryController>().getSlots();

            for(int i = 0; i < inventory.Length; i++){
                if(!inventory[i]){
                    print(i);
                    print(obj);
                    print(cant);
                    GameObject.FindGameObjectWithTag("GenerateEvent").GetComponent<InventoryController>().setSlots(i, obj, cant);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
