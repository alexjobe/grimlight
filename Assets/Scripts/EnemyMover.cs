using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float chaseAggroRange = 10f;
    public bool shouldChase = true;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private Vector2 moveDirection;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (shouldChase && PlayerController.Instance.gameObject.activeInHierarchy)
        {
            if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < chaseAggroRange)
            {
                animator.SetBool("isChasing", true);
                moveDirection = PlayerController.Instance.transform.position - transform.position;
            }
            else
            {
                animator.SetBool("isChasing", false);
                moveDirection = Vector2.zero;
            }

            moveDirection.Normalize();
            rigidBody.velocity = moveDirection * moveSpeed;
        }
        else
        {
            animator.SetBool("isChasing", false);
            rigidBody.velocity = Vector2.zero;
        }
    }
}
