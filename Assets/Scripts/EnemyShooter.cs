using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public bool shouldFire = true;
    public float projectileAggroRange = 10f;
    public float projectileCooldown = 1f;
    public EnemyProjectile projectileToFire;

    private IEnumerator projectileCooldownCounter;
    private SpriteRenderer spriteRenderer;

    private bool isProjectileOnCooldown = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(spriteRenderer.isVisible && PlayerController.Instance.gameObject.activeInHierarchy) 
        {
            TryToFire();
        }
    }

    void TryToFire()
    {
        if (shouldFire && !isProjectileOnCooldown && Vector2.Distance(transform.position, 
            PlayerController.Instance.transform.position) < projectileAggroRange)
        {
            Fire();
        }
    }

    void Fire()
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
