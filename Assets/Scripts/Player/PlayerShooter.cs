using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public float projectileCooldown = 0.5f;
    public PlayerProjectile projectileToFire;
    public GameObject fireSpot;

    private Vector2 aimInput;
    private bool isTryingToFire = false;
    private IEnumerator projectileCooldownCounter;

    private void Update()
    {
        UpdateFiringInput();
        ProcessFiring();
    }

    private void UpdateFiringInput()
    {
        if (!PlayerController.Instance.isInMelee)
        {
            aimInput = new Vector2(Input.GetAxisRaw("HorizontalAim"), Input.GetAxisRaw("VerticalAim"));
            aimInput.Normalize();
            FaceTowardsAim();
            isTryingToFire = aimInput != Vector2.zero ? true : false;
        }
    }

    private void FaceTowardsAim()
    {
        if(!PlayerController.Instance.isProjectileOnCooldown)
        {
            if (aimInput.x > 0)
            {
                PlayerController.Instance.facing = Facing.Right;
            }
            else if (aimInput.x < 0)
            {
                PlayerController.Instance.facing = Facing.Left;
            }
            else if (aimInput.y < 0)
            {
                PlayerController.Instance.facing = Facing.Up;
            }
            else if (aimInput.y > 0)
            {
                PlayerController.Instance.facing = Facing.Down;
            }
        }
    }

    private void ProcessFiring()
    {
        if (!PlayerController.Instance.isProjectileOnCooldown && !PlayerController.Instance.isDashing && isTryingToFire)
        {
            PlayerProjectile projectile = Instantiate(projectileToFire, fireSpot.transform.position, fireSpot.transform.rotation);
            projectileCooldownCounter = ProjectileCooldownCounter();
            projectile.direction = GetProjectileDirection();
            StartCoroutine(projectileCooldownCounter);
        }
    }

    private Vector2 GetProjectileDirection()
    {
        Vector2 direction;

        switch (PlayerController.Instance.facing)
        {
            case Facing.Up:
                direction = Vector2.up;
                break;
            case Facing.Left:
                direction = Vector2.left;
                break;
            case Facing.Right:
                direction = Vector2.right;
                break;
            default:
                direction = Vector2.down;
                break;
        }

        return direction;
    }

    public IEnumerator ProjectileCooldownCounter()
    {
        PlayerController.Instance.isProjectileOnCooldown = true;
        float cooldownCounter = projectileCooldown;

        while (cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            yield return null;
        }

        PlayerController.Instance.isProjectileOnCooldown = false;
    }
}
