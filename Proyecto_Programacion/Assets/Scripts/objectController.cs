using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectController : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private int cantidad = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
}
