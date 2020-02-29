using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float chaseAggroRange = 7f;
    public bool shouldChase = true;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerInstance;
    
    private Vector2 moveDirection;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInstance = PlayerController.Instance;
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (shouldChase && playerInstance.gameObject.activeInHierarchy)
        {
            if (Vector2.Distance(transform.position, playerInstance.transform.position) < chaseAggroRange)
            {
                animator.SetBool("isChasing", true);
                moveDirection = playerInstance.transform.position - transform.position;
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
