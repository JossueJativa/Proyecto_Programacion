using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float cooldownAtact;
    private Rigidbody2D rb;
    [SerializeField] private float velocity;
    [SerializeField] private Vector2 direction = Vector2.right;
    [SerializeField] private float timeTomove;
    private Vector2[] directionPosibles = {Vector2.right, Vector2.left};

    private bool canJump = true;
    private bool canAtact = true;

    public float jumpInterval;

    [SerializeField] private float vida;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(CambiarDireccionPeriodicamente());
    }

    private void Update()
    {
        movement();
    }
    
    private void movement(){
        transform.Translate(direction * velocity * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            if(!canAtact) return;
            canAtact = false;

            other.gameObject.GetComponent<PlayerController>().DamageApply(10);
            Invoke("ActiveAtact", cooldownAtact);
        }
    }

    void ActiveAtact(){
        canAtact = true;
    }

    private IEnumerator CambiarDireccionPeriodicamente()
    {
        while (true)
        {
            // Selecciona una nueva dirección aleatoria
            direction = directionPosibles[Random.Range(0, directionPosibles.Length)];

            // Espera el tiempo especificado antes de cambiar de dirección nuevamente
            yield return new WaitForSeconds(timeTomove);
        }
    }

    public void Takedamage(float damage){
        vida -= damage;

        if(vida <= 0){
            TimeToDead();
            Destroy(gameObject);
        }
    }

    private IEnumerable TimeToDead(){
        yield return new WaitForSeconds(0.1f);
    }
}
