using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}

public class PlayerController : MonoBehaviour
{

    public Facing facing { get; set; }

    public bool isProjectileOnCooldown { get; set; } = false;
    public bool isIdle { get; set; } = true;

    public bool isDashing { get; set; } = false;
	public bool isDashOnCooldown { get; set; } = false;

    public bool isInMelee { get; set; } = false;
    public bool isMeleeOnCooldown { get; set; } = false;

    private static PlayerController instance;
    private Animator animator;

    public static PlayerController Instance
    {
        get { return instance; }
    }

    private void Awake() 
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

    private void Start()
    {
        animator = GetComponent<Animator>();
        facing = Facing.Down;
    }

    private void Update()
    {
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        if (isIdle)
        {
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }

        if (PlayerController.Instance.isDashing)
        {
            animator.SetBool("isDashing", true);
        }
        else
        {
            animator.SetBool("isDashing", false);
        }

        switch (PlayerController.Instance.facing)
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
}
