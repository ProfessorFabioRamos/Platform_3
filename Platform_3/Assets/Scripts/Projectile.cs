using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float timer = 0;
    public float lifeTime = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
        //Invoke("DestroyProjectile", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        timer += Time.deltaTime;
        Debug.Log(timer);
        if(timer >= lifeTime){
            DestroyProjectile();
        }
        */
    }

    void DestroyProjectile(){
        //Destroy(this.gameObject);
        Destroy(gameObject);
    }
}
