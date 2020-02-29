using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public bool shouldFire = true;
    public float projectileAggroRange = 10f;
    public float projectileCooldown = 1f;
    public EnemyProjectile projectileToFire;

    private SpriteRenderer spriteRenderer;
    private IEnumerator projectileCooldownCounter;
    private bool isProjectileOnCooldown = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(spriteRenderer.isVisible && PlayerController.Instance.gameObject.activeInHierarchy) 
        {
            TryToFire();
        }
    }

    private void TryToFire()
    {
        if (shouldFire && !isProjectileOnCooldown && Vector2.Distance(transform.position, 
            PlayerController.Instance.transform.position) < projectileAggroRange)
        {
            Fire();
        }
    }

    private void Fire()
    {
        EnemyProjectile projectile = Instantiate(projectileToFire, transform.position, transform.rotation);
        projectileCooldownCounter = ProjectileCooldownCounter();
        StartCoroutine(projectileCooldownCounter);
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
}
