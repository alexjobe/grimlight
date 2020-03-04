using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public float chaseMoveSpeed = 2f;
    public float chaseAggroRange = 7f;
    public bool shouldChase = true;

    public float knockBackSpeed = 3f;
    public float knockBackDuration = 0.1f;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerInstance;
    
    private Vector2 moveDirection;
    private float currentMoveSpeed;
    private bool isKnockedBack = false;
    private IEnumerator knockBackDurationCounter;

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
        rigidBody.velocity = moveDirection * currentMoveSpeed;
    }

    private void UpdateMovement()
    {
        if(isKnockedBack)
        {
            currentMoveSpeed = knockBackSpeed;
        }
        else if (shouldChase && playerInstance.gameObject.activeInHierarchy)
        {
            ChasePlayer();
        }
        else
        {
            animator.SetBool("isChasing", false);
            currentMoveSpeed = 0f;
        }
    }

    private void ChasePlayer()
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

        currentMoveSpeed = chaseMoveSpeed;
        moveDirection.Normalize();
    }

    public void KnockBack(Vector3 sourceOfKnockback)
    {
        moveDirection = transform.position - sourceOfKnockback;
        moveDirection.Normalize();
        knockBackDurationCounter = KnockBackDurationCounter();
        StartCoroutine(knockBackDurationCounter);
    }

    public IEnumerator KnockBackDurationCounter()
    {
        isKnockedBack = true;
        float durationCounter = 0f;

        while (durationCounter < knockBackDuration)
        {
            durationCounter += Time.deltaTime;
            yield return null;
        }

        isKnockedBack = false;
    }
}
