using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 8f;
    private Vector3 direction;
    public GameObject explodeEffect;

    void Start()
    {
        direction = PlayerController.Instance.transform.position - transform.position;
        direction.Normalize();
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Instantiate(explodeEffect, transform.position, transform.rotation);
        
        if(other.tag == "Player")
        {
            PlayerHealthController.Instance.DamagePlayer();
        }
        
        Destroy(gameObject);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
