using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject[] slots;
    private Text text;

    private int num_slots = 13;
    void Start()
    {
        slots = new GameObject[num_slots];
    }

    void Update()
    {
        
    }

    public GameObject[] getSlots(){
        return this.slots;
    }

    public void setSlots(int pos, GameObject slot, int cant){
        bool exist = false;

        for (int i = 0; i < slots.Length; i++){
            if (slots[i] != null && slots[i].tag == slot.tag){
                int already_cant = slots[i].GetComponent<AtributesController>().getCant();
                slots[i].GetComponent<AtributesController>().setCant(already_cant + cant);
                exist = true;
                break;
            }
        }

        if(!exist){
            slot.GetComponent<AtributesController>().setCant(cant);
            this.slots[pos] = slot;
        }
    }

    public void showInventory(){
        Component[] inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponentsInChildren<Transform>();
        bool slotIsUsed = false;

        for(int i = 0; i < slots.Length; i++){
            if(slots[i] != null){
                slotIsUsed = false;

                for(int j = 0; j < inventory.Length; j++){
                    GameObject child = inventory[j].gameObject;

                    if(child.CompareTag("Slot") && child.transform.childCount <= 1 && !slotIsUsed){
                        GameObject item = Instantiate(slots[i], child.transform.position, Quaternion.identity);
                        item.transform.SetParent(child.transform, false);
                        item.transform.localPosition = new Vector3(0, 0, 0);

                        item.name = item.name.Replace("(Clone)", "");
                        item.GetComponentInChildren<Text>().text = item.name;
                        int cant = 1;

                        text.text = cant.ToString(); 
                        slotIsUsed = true;              
                    }
                }
            }
        }
    }

    public bool removeSlot(GameObject[] obj){
        for(int i = 1; i < obj.Length; i++){
            GameObject child = obj[i].gameObject;
            if(child.tag == "Slot" && child.transform.childCount > 0){
                for(int e = 0; e <= 0; e++){
                    Destroy(child.transform.GetChild(e).transform.gameObject);
                }
            }
        }

        return true;
    }
}
