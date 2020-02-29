using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public float meleeDuration = 1f;
    public float meleeCooldown = 3f;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private PlayerController playerInstance;

    private IEnumerator meleeDurationCounter;
    private IEnumerator meleeCooldownCounter;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInstance = PlayerController.Instance;
    }

    private void Update()
    {
        UpdateMeleeInput();
    }

    private void UpdateMeleeInput()
    {
        if (!playerInstance.isMeleeOnCooldown)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                rigidBody.velocity = Vector2.zero;
                playerInstance.facing = Facing.Down;
                animator.SetTrigger("meleeTrigger");
                meleeDurationCounter = MeleeDurationCounter();
                meleeCooldownCounter = MeleeCooldownCounter();
                StartCoroutine(meleeDurationCounter);
                StartCoroutine(meleeCooldownCounter);
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
}
