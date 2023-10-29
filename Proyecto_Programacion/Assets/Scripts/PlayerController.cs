using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //Player mechanics
    public float speed;
    public float jump;
    public LayerMask floorCap;
    public float dash;
    public float timeDash;
    // public int max_jumps;

    //Enemys interactions
    public float forceDirection;

    //Habilities conditions
    private bool hasDash = true;
    private bool canMove = true;
    public GameObject bloquePrefab;
    private bool canMoveDamage = true;
    private SpriteRenderer spriteRenderer;
    // private int jumps_given;

    //Unity items
    private new Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider2D;
    private bool orient = true;
    private Animator animator;

    //Elementos del bloque
    public float timeToDestroy;
    private bool blockPlaced = false;
    private List<GameObject> instantiatedBlocks = new List<GameObject>();

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // max_jumps = jumps_given;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jumping();
        PutBlocks();
    }

    //player habilities
    private void Movement(){
        //Movement logic
        if(!canMoveDamage)return;

        float isClicked = Input.GetAxis("Horizontal");
        //Teclas de input, -1 izquierda, +1 Derecha

        if(isClicked != 0f){
            animator.SetBool("isRunning", true);
        }
        else{
            animator.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && canMove){
            StartCoroutine(PlayerDash());
        }

        if(canMove){
            rigidbody.velocity = new Vector2(isClicked * speed, rigidbody.velocity.y);
        }
        Orientation(isClicked);
    }

    private void Jumping(){
        //Saltos en la logica
        if (Input.GetKeyDown(KeyCode.Space) && isOnFloor()){
            rigidbody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        }
    }

    private IEnumerator PlayerDash(){
        canMove = false;
        hasDash = false;
        float isClicked = Input.GetAxis("Horizontal");
        animator.SetTrigger("Dash");

        rigidbody.velocity = new Vector2(dash * OrientationNumber(isClicked), 0);

        yield return new WaitForSeconds(timeDash);

        canMove = true;
        hasDash = true;
    }

    /* void hasDobleJumps(){
        if (isOnFloor()){
            jumps_given = max_jumps;
        }

        if (Input.GetKeyDown(KeyCode.Space) && (jumps_given > 0)){
            max_jumps --;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            rigidbody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        }
    } */

    //Logic game
    private void Orientation(float inputMovement){
        //Orientation logic
        if ( ( (orient == true ) && (inputMovement < 0) ) || ((orient == false ) && (inputMovement > 0)) ){
            orient = !orient;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    private int OrientationNumber(float inputMovement){
        //Orientation logic
        if ( ( (orient == true ) && (inputMovement < 0) ) || ((orient == false ) && (inputMovement > 0)) ){
            orient = !orient;
        }

        if (orient){
            return 1;
        }
        else{
            return -1;
        }
    }

    private bool isOnFloor(){
        //Si esta tocando el suelo
        RaycastHit2D raycastHit2D =  Physics2D.BoxCast(boxCollider2D.bounds.center, new Vector2(boxCollider2D.bounds.size.x, boxCollider2D.bounds.size.y), 0f, 
        Vector2.down, 0.2f, floorCap);

        return raycastHit2D.collider != null;
    }

    private void PutBlocks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPos.z = 0;

            // Crea una nueva instancia del bloque y almacénala en instantiatedBlock.
            GameObject newBlock = Instantiate(bloquePrefab, clickPos, Quaternion.identity);
            instantiatedBlocks.Add(newBlock);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPos.z = 0;

            // Busca y destruye el bloque que esté en la posición del clic derecho.
            GameObject blockToDestroy = null;
            foreach (GameObject block in instantiatedBlocks)
            {
                if (Vector3.Distance(block.transform.position, clickPos) < 0.5f) // Ajusta el valor según el tamaño de tus bloques.
                {
                    blockToDestroy = block;
                    break;
                }
            }

            if (blockToDestroy != null)
            {
                instantiatedBlocks.Remove(blockToDestroy);
                Destroy(blockToDestroy);
            }
        }
    }

    //ENemys interactions
    public void DamageApply(){
        canMoveDamage = false;

        Vector2 kickDirection;

        if (rigidbody.velocity.x > 0){
            kickDirection = new Vector2(1, 1);
        }
        else{
            kickDirection = new Vector2(-1,1);
        }
        rigidbody.AddForce(kickDirection * forceDirection);

        StartCoroutine(WaitAndDoMovement(0.5f)); // Pasa 1.0f como el tiempo que se espera.
    }

    IEnumerator WaitAndDoMovement(float duration){
        Color originalColor = spriteRenderer.color;
        Color damagedColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        spriteRenderer.color = damagedColor;

        yield return new WaitForSeconds(duration); // Espera el tiempo especificado.

        spriteRenderer.color = originalColor; // Restaura el color original.

        while (!isOnFloor()){
            yield return null;
        }

        canMoveDamage = true;
    }
}
