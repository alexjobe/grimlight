using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public float meleeDuration = 1f;
    public float meleeCooldown = 3f;
    public int damage = 30;
    public float attackRange = 3f;
    public LayerMask enemyLayerMask;
    public GameObject meleeZone;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private PlayerController playerInstance;

    private IEnumerator meleeDurationCounter;
    private IEnumerator meleeCooldownCounter;

    private HashSet<int> alreadyDamagedEnemies;

    private void Start()
    {
        alreadyDamagedEnemies = new HashSet<int>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInstance = PlayerController.Instance;
    }

    private void Update()
    {
        UpdateMeleeInput();

        if(playerInstance.isInMelee)
        {
            DamageEnemies();
        }
    }

    private void UpdateMeleeInput()
    {
        if (!playerInstance.isMeleeOnCooldown)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartMeleeAttack();
            }
        }
    }

    private void StartMeleeAttack()
    {
        alreadyDamagedEnemies.Clear();
        animator.SetTrigger("meleeAttackTrigger");
        meleeZone.SetActive(true);
        playerInstance.facing = Facing.Down;
        rigidBody.velocity = Vector2.zero;
        meleeDurationCounter = MeleeDurationCounter();
        meleeCooldownCounter = MeleeCooldownCounter();
        StartCoroutine(meleeDurationCounter);
        StartCoroutine(meleeCooldownCounter);
    }

    private void DamageEnemies()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayerMask);

        foreach (Collider2D enemy in enemiesToDamage)
        {
            // Only damage an enemy once per attack
            if(!alreadyDamagedEnemies.Contains(enemy.GetInstanceID()))
            {
                alreadyDamagedEnemies.Add(enemy.GetInstanceID());
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                enemy.GetComponent<EnemyMover>().KnockBack(transform.position);
            }
        }
    }

    public IEnumerator MeleeDurationCounter()
    {
        playerInstance.isInMelee = true;
        float durationCounter = 0f;

        while (durationCounter < meleeDuration)
        {
            durationCounter += Time.deltaTime;
            yield return null;
        }

        meleeZone.SetActive(false);
        playerInstance.isInMelee = false;
    }

    public IEnumerator MeleeCooldownCounter()
    {
        playerInstance.isMeleeOnCooldown = true;
        float cooldownCounter = meleeCooldown;

        while (cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            yield return null;
        }

        playerInstance.isMeleeOnCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
