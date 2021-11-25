using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float direction = -1.0f;
    public float speed = 2.0f;
    public int enemyHP = 2;
    public int damage = 1;
    private Rigidbody2D rig;
    private SpriteRenderer spr;
    private Animator anim;
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        rig.velocity = new Vector2(speed*direction, rig.velocity.y);
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if(distance <= 2){
            anim.SetTrigger("attack");
        }
    }

    public void DamagePlayer(){
        playerTransform.GetComponent<Player>().TakeDamage(damage);
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.layer == 8){
            direction *= -1;
            Flip();
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Projectile"){
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    void Flip(){
        spr.flipX = !spr.flipX;
    }

    public void TakeDamage(int damage){
        enemyHP -= damage;
        if(enemyHP <= 0){
            anim.SetTrigger("dead");
            Destroy(gameObject, 2);
        }
    }
}
