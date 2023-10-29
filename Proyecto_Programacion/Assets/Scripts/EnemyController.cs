using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float cooldownAtact;
    private bool canAtact = true;
    private Rigidbody2D rb;
    private bool canJump = true;
    public float moveSpeed;
    public float jumpForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movementEnemy();
    }

    private void movementEnemy(){
        // Mover automáticamente el slime horizontalmente
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        // Permitir saltar si el slime está en el suelo
        if (canJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            if(!canAtact) return;
            canAtact = false;

            other.gameObject.GetComponent<PlayerController>().DamageApply();
            Invoke("ActiveAtact", cooldownAtact);
        }
    }

    void ActiveAtact(){
        canAtact = true;
    }
}
