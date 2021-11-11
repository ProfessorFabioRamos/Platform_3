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

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rig.velocity = new Vector2(speed*direction, rig.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.layer == 9){
            direction *= -1;
            Flip();
        }
    }

    void Flip(){
        spr.flipX = !spr.flipX;
    }

    public void TakeDamage(int damage){
        enemyHP -= damage;
        if(enemyHP <= 0){
            Destroy(this.gameObject);
        }
    }
}
