using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerInstance;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerInstance = PlayerController.Instance;
    }

    private void Update()
    {
        UpdateMoveAnimation();
        UpdateFacingAnimation();
    }

    private void UpdateMoveAnimation()
    {
        if (playerInstance.isIdle)
        {
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }

        if (playerInstance.isDashing)
        {
            animator.SetBool("isDashing", true);
        }
        else
        {
            animator.SetBool("isDashing", false);
        }
    }

    private void UpdateFacingAnimation()
    {
        switch (playerInstance.facing)
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
