using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public float projectileCooldown = 0.5f;
    public PlayerProjectile projectileToFire;
    public GameObject fireSpot;

    private PlayerController playerInstance;

    private Vector2 aimInput;
    private bool isTryingToFire = false;
    private IEnumerator projectileCooldownCounter;

    private void Start() {
        playerInstance = PlayerController.Instance;
    }

    private void Update()
    {
        UpdateFiringInput();
        ProcessFiring();
    }

    private void UpdateFiringInput()
    {
        if (!playerInstance.isInMelee)
        {
            aimInput = new Vector2(Input.GetAxisRaw("HorizontalAim"), Input.GetAxisRaw("VerticalAim"));
            aimInput.Normalize();
            FaceTowardsAim();
            isTryingToFire = aimInput != Vector2.zero ? true : false;
        }
    }

    private void FaceTowardsAim()
    {
        if(!playerInstance.isProjectileOnCooldown)
        {
            if (aimInput.x > 0)
            {
                playerInstance.facing = Facing.Right;
            }
            else if (aimInput.x < 0)
            {
                playerInstance.facing = Facing.Left;
            }
            else if (aimInput.y < 0)
            {
                playerInstance.facing = Facing.Up;
            }
            else if (aimInput.y > 0)
            {
                playerInstance.facing = Facing.Down;
            }
        }
    }

    private void ProcessFiring()
    {
        if (!playerInstance.isProjectileOnCooldown && !playerInstance.isDashing && isTryingToFire)
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

        switch (playerInstance.facing)
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
        playerInstance.isProjectileOnCooldown = true;
        float cooldownCounter = projectileCooldown;

        while (cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            yield return null;
        }

        playerInstance.isProjectileOnCooldown = false;
    }
}
