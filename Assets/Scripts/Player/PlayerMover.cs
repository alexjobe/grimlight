using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public float normalMoveSpeed = 4f;
    public float dashMoveSpeed = 8f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 2f;

    private Rigidbody2D rigidBody;
    private PlayerController playerInstance;

    private Vector2 moveInput;
    private float activeMoveSpeed;

    private IEnumerator dashDurationCounter;
    private IEnumerator dashCooldownCounter;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerInstance = PlayerController.Instance;
        activeMoveSpeed = normalMoveSpeed;
    }

    private void Update()
    {
        UpdateDashInput();
        UpdateMovement();
        UpdateFacing();
    }

    private void UpdateMovement()
    {
        if (!playerInstance.isInMelee)
        {
            moveInput.x = Input.GetAxisRaw("HorizontalMove");
            moveInput.y = Input.GetAxisRaw("VerticalMove");
            moveInput.Normalize();
            rigidBody.velocity = moveInput * activeMoveSpeed;

            playerInstance.isIdle = rigidBody.velocity == Vector2.zero ? true : false;
        }
    }

    private void UpdateDashInput()
    {
        if (!playerInstance.isInMelee && !playerInstance.isDashOnCooldown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                activeMoveSpeed = dashMoveSpeed;
                dashDurationCounter = DashDurationCounter();
                dashCooldownCounter = DashCooldownCounter();
                StartCoroutine(dashDurationCounter);
                StartCoroutine(dashCooldownCounter);
            }
        }
    }

    private void UpdateFacing()
    {
        if (!playerInstance.isProjectileOnCooldown && !playerInstance.isInMelee)
        {
            if (moveInput.x > 0)
            {
                playerInstance.facing = Facing.Right;
            }
            else if (moveInput.x < 0)
            {
                playerInstance.facing = Facing.Left;
            }
            else if (moveInput.y > 0)
            {
                playerInstance.facing = Facing.Up;
            }
            else if (moveInput.y < 0)
            {
                playerInstance.facing = Facing.Down;
            }
        }
    }

    public IEnumerator DashDurationCounter()
    {
        playerInstance.isDashing = true;
        float durationCounter = 0f;

        while (durationCounter < dashDuration)
        {
            durationCounter += Time.deltaTime;
            yield return null;
        }

        activeMoveSpeed = normalMoveSpeed;
        playerInstance.isDashing = false;
    }

    public IEnumerator DashCooldownCounter()
    {
        playerInstance.isDashOnCooldown = true;
        float cooldownCounter = dashCooldown;

        while (cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            yield return null;
        }

        playerInstance.isDashOnCooldown = false;
    }
}
