using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("Movimentação")]
    #region Movimentação
    public float horizontalSpeed = 3.0f;
    public int direction = 1;
    #endregion
    [Header ("Pulo")]
    #region Pulo
    public float jumpForce = 500;
    public LayerMask whatIsFloor;
    #endregion
    [Header ("Ataques")]
    #region Ataques
    public BoxCollider2D attackArea;
    public int melleDamage = 1;
    private EnemyBase enemyInArea = null;

    #endregion
    [Header ("Referencias")]
    #region Referencias de Componentes
    private Rigidbody2D rig;
    private Animator anim;
    private SpriteRenderer spr;
    #endregion
    

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        attackArea = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");

        if(h>0) Flip(true);
        else if(h<0)Flip(false);

        anim.SetFloat("speed", Mathf.Abs(h));

        rig.velocity = new Vector2(h*horizontalSpeed, rig.velocity.y);

        bool grounded = Physics2D.OverlapCircle(transform.position, 0.2f, whatIsFloor);

        if(grounded && Input.GetButtonDown("Jump")){
            rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        anim.SetBool("grounded", grounded);

        if(Input.GetKeyDown(KeyCode.X)){
            anim.SetTrigger("attack");
        }
    }

    void Flip(bool faceRight){
        spr.flipX = !faceRight;
        if(faceRight) direction = 1;
        else direction = -1;
        attackArea.offset = new Vector2(direction, attackArea.offset.y);
    }

    public void DamageEnemy(){
        if(enemyInArea != null){
            enemyInArea.TakeDamage(1);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.layer == 12){
            enemyInArea = other.GetComponent<EnemyBase>();
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.layer == 12){
            enemyInArea = null;
        }
    }
}
