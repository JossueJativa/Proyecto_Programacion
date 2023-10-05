using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detecta la colisión con el personaje
        if (collision.gameObject.CompareTag("Personaje"))
        {
            // Destruye el bloque cuando colisiona con el personaje
            Destroy(gameObject);
        }
    }
}
