using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public GameObject explodeEffect;
    public float speed = 8f;
    public Vector2 direction = Vector2.zero;
    public int damage = 10;

    private Rigidbody2D rigidBody;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidBody.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        Instantiate(explodeEffect, transform.position, transform.rotation);
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
