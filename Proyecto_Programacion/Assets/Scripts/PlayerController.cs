using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Public variables
    public float fast;
    public float jump;
    public LayerMask floorCap;
    // public int max_jumps;

    //Private variables
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider2D;
    private bool orient = true;
    private Animator animator;
    // private int jumps_given; 
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

    void Movement(){
        //Movement logic
        float isClicked = Input.GetAxis("Horizontal");
        //Teclas de input, -1 izquierda, +1 Derecha

        if(isClicked != 0f){
            animator.SetBool("isRunning", true);
        }
        else{
            animator.SetBool("isRunning", false);
        }

        rigidbody.velocity = new Vector2(isClicked * fast, rigidbody.velocity.y);
        Orientation(isClicked);
    }

    void Orientation(float inputMovement){
        //Orientation logic
        if ( ( (orient == true ) && (inputMovement < 0) ) || ((orient == false ) && (inputMovement > 0)) ){
            orient = !orient;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    bool isOnFloor(){
        //Si esta tocando el suelo
        RaycastHit2D raycastHit2D =  Physics2D.BoxCast(boxCollider2D.bounds.center, new Vector2(boxCollider2D.bounds.size.x, boxCollider2D.bounds.size.y), 0f, 
        Vector2.down, 0.2f, floorCap);

        return raycastHit2D.collider != null;
    }

    void Jumping(){
        //Saltos en la logica
        if (Input.GetKeyDown(KeyCode.Space) && isOnFloor()){
            rigidbody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
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
}
