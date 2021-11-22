using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float direction = -1.0f;
    public float speed = 2.0f;
    public int enemyHP = 2;
    private Rigidbody2D rig;
    private SpriteRenderer spr;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rig.velocity = new Vector2(speed*direction, rig.velocity.y);
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
