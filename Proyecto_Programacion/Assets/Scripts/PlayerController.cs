using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    //Player mechanics
    public float speed;
    public float jump;
    public LayerMask floorCap;
    public float dash;
    public float timeDash;
    [SerializeField] private float life;
    [SerializeField] private float maxLife;
    [SerializeField] private COntrollerVida barraVida;
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
    private int selectedSlotIndex;

    //Elementos del bloque
    public float timeToDestroy;
    GameObject inventory_com;

    //PVP
    [SerializeField] private Transform controllerPunch;
    [SerializeField] private float radioPunch;
    [SerializeField] private float timesToAtact;
    [SerializeField] private float nexAtact;
    [SerializeField] private float damageForEnemy;
    private bool can_build;
    private ConstructionController constructionController;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        life = maxLife;
        barraVida.StartLife(life);
        inventory_com = GameObject.FindGameObjectWithTag("Inventory");
        constructionController = GetComponent<ConstructionController>();
        // Asegúrate de que la referencia no sea null antes de usarla
        if (constructionController == null)
        {
            Debug.LogError("ConstructionController not found on the PlayerController GameObject.");
        }
        // max_jumps = jumps_given;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jumping();
        AttacEnemys();
        SelectItemInventory();
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

    private void AttacEnemys()
    {
        if (Input.GetMouseButtonDown(0) && nexAtact <= 0)
        {
            Golpe();
            nexAtact = timesToAtact;
        }
        else if(nexAtact > 0){
            nexAtact -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPos.z = 0;
        }
    }

    //Enemys interactions
    public void DamageApply(float damage){
        canMoveDamage = false;

        Vector2 kickDirection;

        // Quitar daño
        life -= damage;
        barraVida.ChangeLife(life);
        if (life <= 0){
            StartCoroutine(WaitTimeDead(0.6f));
        }

        if (rigidbody.velocity.x > 0){
            kickDirection = new Vector2(1, 1);
        }
        else{
            kickDirection = new Vector2(-1,1);
        }
        rigidbody.AddForce(kickDirection * forceDirection);
        StartCoroutine(WaitAndDoMovement(0.7f));
    }

    IEnumerator WaitAndDoMovement(float duration){
        animator.SetTrigger("Damage");
        yield return new WaitForSeconds(duration);

        while (!isOnFloor()){
            yield return null;
        }

        canMoveDamage = true;
    }

    IEnumerator WaitTimeDead(float duration){
        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(duration);

        while (!isOnFloor()){
            yield return null;
        }

        Destroy(gameObject);
    }

    //Damage
    private void Golpe(){
        animator.SetTrigger("golpe");
        
        Collider2D[] objectos = Physics2D.OverlapCircleAll(controllerPunch.position, radioPunch);

        foreach(Collider2D collition in objectos){
            if(collition.CompareTag("Enemy")){
                collition.transform.GetComponent<EnemyController>().Takedamage(damageForEnemy);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(controllerPunch.position, radioPunch);
    }

    private void SelectItemInventory(){
        GameObject[] slots = GameObject.FindGameObjectsWithTag("Slot")
                                    .OrderBy(slot => slot.name)
                                    .ToArray();
        if(Input.GetKeyUp(KeyCode.Alpha1)){
            if(slots[0].transform.childCount > 0){
                slots[0].GetComponentInChildren<AtributesController>().Action();
            }
        }
        else if(Input.GetKeyUp(KeyCode.Alpha2)){
            if(slots[1].transform.childCount > 0){
                slots[1].GetComponentInChildren<AtributesController>().Action();
            }
        }
        else if(Input.GetKeyUp(KeyCode.Alpha3)){
            if(slots[2].transform.childCount > 0){
                slots[2].GetComponentInChildren<AtributesController>().Action();
            }
        }
        else if(Input.GetKeyUp(KeyCode.Alpha4)){
            if(slots[3].transform.childCount > 0){
                slots[3].GetComponentInChildren<AtributesController>().Action();
            }
        }
        else if(Input.GetKeyUp(KeyCode.Alpha5)){
            if(slots[4].transform.childCount > 0){
                slots[4].GetComponentInChildren<AtributesController>().Action();
            }
        }
        else if(Input.GetKeyUp(KeyCode.Alpha6)){
            if(slots[5].transform.childCount > 0){
                slots[5].GetComponentInChildren<AtributesController>().Action();
            }
        }
        else if(Input.GetKeyUp(KeyCode.Alpha7)){
            if(slots[6].transform.childCount > 0){
                slots[6].GetComponentInChildren<AtributesController>().Action();
            }
        }
        else if(Input.GetKeyUp(KeyCode.Alpha8)){
            if(slots[7].transform.childCount > 0){
                slots[7].GetComponentInChildren<AtributesController>().Action();
            }
        }
        else if(Input.GetKeyUp(KeyCode.Alpha9)){
            if(slots[8].transform.childCount > 0){
                slots[8].GetComponentInChildren<AtributesController>().Action();
            }
        }
        else if(Input.GetKeyUp(KeyCode.Alpha0)){
            if(slots[9].transform.childCount > 0){
                slots[9].GetComponentInChildren<AtributesController>().Action();
            }
        }
        else if(Input.GetKeyUp(KeyCode.X)){
            if(slots[10].transform.childCount > 0){
                slots[10].GetComponentInChildren<AtributesController>().Action();
            }
        }
        else if(Input.GetKeyUp(KeyCode.C)){
            if(slots[11].transform.childCount > 0){
                slots[11].GetComponentInChildren<AtributesController>().Action();
            }
        }
    }
}
