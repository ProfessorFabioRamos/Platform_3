using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public bool isJumping;
    public float jumpTimeCounter;
    public float jumpTime = 1.5f;
    #endregion
    [Header ("Ataques")]

    #region Ataques
    public BoxCollider2D attackArea;
    public int melleDamage = 1;
    private EnemyBase enemyInArea = null;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    public Transform shootingPosition;
    #endregion
    
    [Header ("Hp e Vida")]
    #region HP e Vida
    public Transform respawnPoint;
    public int maxHP = 10;
    public int HP;
    public Slider hpBar;
    private bool isAlive = true;
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
        HP = maxHP;
        hpBar.maxValue = maxHP;
        hpBar.value = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive){
            float h = Input.GetAxis("Horizontal");

            if(h>0) Flip(true);
            else if(h<0)Flip(false);

            anim.SetFloat("speed", Mathf.Abs(h));

            rig.velocity = new Vector2(h*horizontalSpeed, rig.velocity.y);

            bool grounded = Physics2D.OverlapCircle(transform.position, 0.2f, whatIsFloor);
            /*
            if(grounded && Input.GetButtonDown("Jump")){
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }*/

            if(grounded && Input.GetButtonDown("Jump")){
                isJumping = true;
                jumpTimeCounter = jumpTime;
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
            }

            if(Input.GetButton("Jump") && isJumping){
                if(jumpTimeCounter > 0){
                    jumpTimeCounter -= Time.deltaTime;
                    rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                }else{
                    isJumping = false;
                }
            }

            if(Input.GetButtonUp("Jump")){
                isJumping = false;
            }

            anim.SetBool("grounded", grounded);

            if(Input.GetKeyDown(KeyCode.X)){
                anim.SetTrigger("attack");
            }

            if(Input.GetKeyDown(KeyCode.Z)){
                anim.SetTrigger("shoot");
            }

            hpBar.value = HP;
        }
    }

    public void TakeDamage(int damage){
        HP -= damage;
        if(HP <=0 && isAlive){
            anim.SetTrigger("dead");
            isAlive = false;
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

    public void Shoot(){
        GameObject proj = Instantiate(projectilePrefab, shootingPosition.position, 
        Quaternion.identity);
        Rigidbody2D projrig = proj.GetComponent<Rigidbody2D>();
        projrig.velocity = new Vector2(projectileSpeed*direction,projrig.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Endlevel"){
            other.GetComponent<Animator>().SetTrigger("open");
            StartCoroutine("LoadLevelTime");
        }
    }

    IEnumerator LoadLevelTime(){
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Level_2");
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.layer == 12){
            enemyInArea = other.GetComponent<EnemyBase>();
        }

        if(other.gameObject.tag == "DeathZone"){
            transform.position = respawnPoint.position;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.layer == 12){
            enemyInArea = null;
        }
    }
}
