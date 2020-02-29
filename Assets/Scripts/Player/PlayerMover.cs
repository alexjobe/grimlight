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

    private Vector2 moveInput;
    private float activeMoveSpeed;

    private IEnumerator dashDurationCounter;
    private IEnumerator dashCooldownCounter;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
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
        if (!PlayerController.Instance.isInMelee)
        {
            moveInput.x = Input.GetAxisRaw("HorizontalMove");
            moveInput.y = Input.GetAxisRaw("VerticalMove");
            moveInput.Normalize();
            rigidBody.velocity = moveInput * activeMoveSpeed;

            PlayerController.Instance.isIdle = rigidBody.velocity == Vector2.zero ? true : false;
        }
    }

    private void UpdateDashInput()
    {
        if (!PlayerController.Instance.isInMelee && !PlayerController.Instance.isDashOnCooldown)
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
        if (!PlayerController.Instance.isProjectileOnCooldown && !PlayerController.Instance.isInMelee)
        {
            if (moveInput.x > 0)
            {
                PlayerController.Instance.facing = Facing.Right;
            }
            else if (moveInput.x < 0)
            {
                PlayerController.Instance.facing = Facing.Left;
            }
            else if (moveInput.y > 0)
            {
                PlayerController.Instance.facing = Facing.Up;
            }
            else if (moveInput.y < 0)
            {
                PlayerController.Instance.facing = Facing.Down;
            }
        }
    }

    public IEnumerator DashDurationCounter()
    {
        PlayerController.Instance.isDashing = true;
        float durationCounter = 0f;

        while (durationCounter < dashDuration)
        {
            durationCounter += Time.deltaTime;
            yield return null;
        }

        activeMoveSpeed = normalMoveSpeed;
        PlayerController.Instance.isDashing = false;
    }

    public IEnumerator DashCooldownCounter()
    {
        PlayerController.Instance.isDashOnCooldown = true;
        float cooldownCounter = dashCooldown;

        while (cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            yield return null;
        }

        PlayerController.Instance.isDashOnCooldown = false;
    }
}
