using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionController : MonoBehaviour
{
    private Vector2 positionMouse;
    private bool signX, signY, construct = true;
    private int positionIntX, positionIntY, positionDefX, positionDefY;
    private float positionFloatX, positionFloatY;
    private SpriteRenderer sprite;
    private GameObject ultimateColition;
    private List<GameObject> createdBlocks = new List<GameObject>();  // Lista para almacenar instancias creadas
    [SerializeField] private GameObject[] blocks;
    [SerializeField] private int selector;
    [SerializeField] private float maxBuildDistance = 5f;  
    [SerializeField] private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        OnMouseDrag();
    }

    public void OnMouseDrag()
    {
        positionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (positionMouse.x >= 0)
        {
            signX = true;
        }
        else
        {
            signX = false;
        }

        if (positionMouse.y >= 0)
        {
            signY = true;
        }
        else
        {
            signY = false;
        }

        positionIntX = (int)Math.Abs(positionMouse.x);
        positionIntY = (int)Math.Abs(positionMouse.y);

        positionFloatX = (float)Math.Abs(positionMouse.x) - positionIntX;
        positionFloatY = (float)Math.Abs(positionMouse.y) - positionIntY;

        if (positionFloatX >= 0.5)
        {
            positionDefX = positionIntX + 1;
        }
        else
        {
            positionDefX = positionIntX;
        }

        if (positionFloatY >= 0.5)
        {
            positionDefY = positionIntY + 1;
        }
        else
        {
            positionDefY = positionIntY;
        }

        if (!signX)
        {
            positionDefX *= -1;
        }

        if (!signY)
        {
            positionDefY *= -1;
        }

        transform.position = new Vector2(positionDefX, positionDefY);

        if (Input.GetMouseButtonDown(0) && construct)
        {
            GameObject newBlock = Instantiate(blocks[selector], transform.position, transform.rotation);
            createdBlocks.Add(newBlock);  // Agregar la nueva instancia a la lista
        }
        else if (Input.GetMouseButtonDown(1) && !construct)
        {
            // Verificar si la instancia a destruir está en la lista
            if (createdBlocks.Contains(ultimateColition))
            {
                Destroy(ultimateColition);
                createdBlocks.Remove(ultimateColition);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        sprite.color = new Color(255, 0, 0, 255);
        construct = false;
        ultimateColition = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        sprite.color = new Color(0, 255, 0, 255);
        construct = true;
    }
}
