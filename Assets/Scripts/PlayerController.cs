using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Facing
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}

public class PlayerController : MonoBehaviour
{
    public float normalMoveSpeed = 4f;
    public float timeUntilIdle = 0.25f;

    private Rigidbody2D rigidBody;
    private Animator animator;

    private Vector2 moveInput;
    private float activeMoveSpeed;
    private Facing facing;
    private float timeSinceLastInput = 0f;
    private IEnumerator timeSinceLastInputCounter;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        activeMoveSpeed = normalMoveSpeed;
        facing = Facing.Down;
        StartTimeSinceLastInputCounter();
    }

    private void StartTimeSinceLastInputCounter()
    {
        timeSinceLastInput = timeUntilIdle + 1;
        timeSinceLastInputCounter = IncrementTimeSinceLastInput();
        StartCoroutine(timeSinceLastInputCounter);
    }

    void Update()
    {
        UpdateMovement();
        UpdateTimeSinceLastInput();
        UpdateFacing();
        UpdateAnimations();
    }

    private void UpdateMovement()
    {
        moveInput.x = Input.GetAxisRaw("HorizontalMove");
        moveInput.y = Input.GetAxisRaw("VerticalMove");
        moveInput.Normalize();
        rigidBody.velocity = moveInput * activeMoveSpeed;
    }
    
    private void UpdateTimeSinceLastInput()
    {
        if (moveInput != Vector2.zero)
        {
            timeSinceLastInput = 0;
        }
    }

    private void UpdateFacing()
    {   
        if(moveInput.x > 0)
        {
            facing = Facing.Right;
        }
        else if(moveInput.x < 0)
        {
            facing = Facing.Left;
        }
        else if(moveInput.y > 0)
        {
            facing = Facing.Up;
        }
        else if(moveInput.y < 0)
        {
            facing = Facing.Down;
        }
    }

    private void UpdateAnimations()
    {
        switch (facing)
        {
            case Facing.Up:
                animator.SetInteger("Facing", (int)Facing.Up);
                break;
            case Facing.Down:
                animator.SetInteger("Facing", (int)Facing.Down);
                break;
            case Facing.Left:
                animator.SetInteger("Facing", (int)Facing.Left);
                break;
            case Facing.Right:
                animator.SetInteger("Facing", (int)Facing.Right);
                break;
        }

        if(timeSinceLastInput > timeUntilIdle)
        {
            animator.SetBool("isIdle", true);
        } 
        else 
        {
            animator.SetBool("isIdle", false);
        }
    }

    public IEnumerator IncrementTimeSinceLastInput()
    {
        while (true)
        {
            timeSinceLastInput += Time.deltaTime;
            yield return null;
        }
    }
}
