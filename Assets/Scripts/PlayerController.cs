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
    public float dashMoveSpeed = 8f;
    public float dashDuration= 0.5f;
    public float dashCooldown = 2f;
    public float projectileCooldown = 0.5f;
    public PlayerProjectile projectileToFire;

    private Rigidbody2D rigidBody;
    private Animator animator;

    private static PlayerController instance;

    private Facing facing;
    private Vector2 moveInput;
    private Vector2 aimInput;
    private float activeMoveSpeed;
    private bool isProjectileOnCooldown = false;
    private bool isTryingToFire = false;
    private bool isDashing = false;
	private bool isDashOnCooldown = false;
    private IEnumerator projectileCooldownCounter;
  	private IEnumerator dashDurationCounter;
    private IEnumerator dashCooldownCounter;

    public static PlayerController Instance
    {
        get { return instance; }
    }

    void Awake() 
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            instance = this;
        }
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        activeMoveSpeed = normalMoveSpeed;
        facing = Facing.Down;
    }

    void Update()
    {
        UpdateMovement();
        UpdateFiringInput();
		UpdateDashInput();
        UpdateFacing();
        ProcessFiring();
        UpdateAnimations();
    }

    private void UpdateMovement()
    {
        moveInput.x = Input.GetAxisRaw("HorizontalMove");
        moveInput.y = Input.GetAxisRaw("VerticalMove");
        moveInput.Normalize();
        rigidBody.velocity = moveInput * activeMoveSpeed;
    }

    private void UpdateFiringInput()
    {
        aimInput.x = Input.GetAxisRaw("HorizontalAim");
        aimInput.y = Input.GetAxisRaw("VerticalAim");
        aimInput.Normalize();

        isTryingToFire = aimInput != Vector2.zero ? true : false;
    }

    private void UpdateDashInput()
    {
		if(!isDashOnCooldown)
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
        if(!isProjectileOnCooldown)
        {
            if (aimInput.x > 0)
            {
                facing = Facing.Right;
            }
            else if (aimInput.x < 0)
            {
                facing = Facing.Left;
            }
            else if (aimInput.y < 0)
            {
                facing = Facing.Up;
            }
            else if (aimInput.y > 0)
            {
                facing = Facing.Down;
            }
            else if (moveInput.x > 0)
            {
                facing = Facing.Right;
            }
            else if (moveInput.x < 0)
            {
                facing = Facing.Left;
            }
            else if (moveInput.y > 0)
            {
                facing = Facing.Up;
            }
            else if (moveInput.y < 0)
            {
                facing = Facing.Down;
            }
        }
    }

	private void ProcessFiring()
	{
		if (!isProjectileOnCooldown && !isDashing && isTryingToFire)
		{
			PlayerProjectile projectile = Instantiate(projectileToFire, transform.position, transform.rotation);
			projectileCooldownCounter = ProjectileCooldownCounter();
			StartCoroutine(projectileCooldownCounter);

			switch (facing)
			{
				case Facing.Up:
					projectile.direction = Vector2.up;
					break;
				case Facing.Down:
					projectile.direction = Vector2.down;
					break;
				case Facing.Left:
					projectile.direction = Vector2.left;
					break;
				case Facing.Right:
					projectile.direction = Vector2.right;
					break;
			}
		}
	}

    private void UpdateAnimations()
    {
        if (moveInput == Vector2.zero && !isDashing)
        {
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }

		if (isDashing)
		{
			animator.SetBool("isDashing", true);
		}
		else
		{
			animator.SetBool("isDashing", false);
		}

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
    }

    public IEnumerator ProjectileCooldownCounter()
    {
        isProjectileOnCooldown = true;
        float cooldownCounter = projectileCooldown;

        while (cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            yield return null;
        }

        isProjectileOnCooldown = false;
    }

	public IEnumerator DashDurationCounter()
	{
		isDashing = true;
		float durationCounter = 0f;

		while (durationCounter < dashDuration)
		{
      		durationCounter += Time.deltaTime;
			yield return null;
		}

		activeMoveSpeed = normalMoveSpeed;
		isDashing = false;
	}

	public IEnumerator DashCooldownCounter()
	{
		isDashOnCooldown = true;
		float cooldownCounter = dashCooldown;

		while (cooldownCounter > 0)
		{
			cooldownCounter -= Time.deltaTime;
			yield return null;
		}

		isDashOnCooldown = false;
	}
}
