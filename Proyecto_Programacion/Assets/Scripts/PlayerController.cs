using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //Player mechanics
    public float speed;
    public float jump;
    public LayerMask floorCap;
    public float dash;
    public float timeDash;
    private bool breakBlock;
    public float timeToBreak;
    // public int max_jumps;

    //Habilities conditions
    private bool hasDash = true;
    private bool canMove = true;
    private float timePassBreak;
    // private int jumps_given;

    //Unity items
    private new Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider2D;
    private bool orient = true;
    private Animator animator; 

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        // max_jumps = jumps_given;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jumping();
    }

    //player habilities
    private void Movement(){
        //Movement logic
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

    private void MouseClicked(){
        //Buttons on the mouse (0) = Left (1) = Right (2) = Center
        if (Input.GetMouseButtonDown(0)){
            breakBlock = true;
        }

        if (breakBlock){
            timePassBreak += Time.deltaTime;

            if (timePassBreak >= timeToBreak){
                BreakBlock();
                breakBlock = false;
                timePassBreak = 0;
            }
        }
        else{
            timePassBreak = 0;
        }
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

    private void BreakBlock(){

    }
}
