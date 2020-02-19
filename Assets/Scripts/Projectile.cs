using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;

    public Vector2 direction = Vector2.zero;

    private Rigidbody2D rigidBody;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidBody.velocity = direction * speed;
    }

    // void OnTriggerEnter2D(Collider2D other) 
    // {
    //     Destroy(gameObject);
    // }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
