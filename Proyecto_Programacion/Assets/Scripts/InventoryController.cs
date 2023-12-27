using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        // Asegúrate de que pos sea un índice válido
        if (pos >= 0 && pos < this.slots.Length)
        {
            if (this.slots[pos] != null)
            {
                for (int i = 0; i < this.slots.Length; i++)
                {
                    if (this.slots[i] != null)
                    {
                        if (slot.name == this.slots[i].name)
                        {
                            int already_cant = this.slots[i].GetComponent<AtributesController>().getCant();
                            this.slots[i].GetComponent<AtributesController>().setCant(already_cant + cant);
                            exist = true;
                        }
                    }
                }
            }

            if (!exist)
            {
                print(pos);
                slot.GetComponent<AtributesController>().setCant(cant);
                this.slots[pos] = slot;
            }
        }
        else
        {
            Debug.LogError("Invalid index: " + pos);
        }
    }

    public void showInventory(){
        GameObject[] slots = GameObject.FindGameObjectsWithTag("Slot")
                                        .OrderBy(slot => slot.name)
                                        .ToArray();

        for (int i = 0; i < slots.Length; i++)
        {
            GameObject slot = slots[i];

            // Verificar si el slot no tiene hijos
            if (slot.transform.childCount == 0)
            {
                if (i < this.slots.Length && this.slots[i] != null)
                {
                    // Verifica si el prefab no es nulo
                    GameObject item = Instantiate(this.slots[i], slot.transform.position, Quaternion.identity);
                    item.transform.SetParent(slot.transform, false);
                    item.transform.localPosition = new Vector3(0, 0, 0);
                    item.name = item.name.Replace("Clone", "");
                    item.name = item.name.Replace("()", "");
                    text = item.GetComponentInChildren<Text>();
                    int cant = 1;
                    text.text = cant + "";
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
