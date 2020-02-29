using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public float meleeDuration = 1f;
    public float meleeCooldown = 3f;

    private Rigidbody2D rigidBody;
    private Animator animator;

    private IEnumerator meleeDurationCounter;
    private IEnumerator meleeCooldownCounter;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateMeleeInput();
    }

    private void UpdateMeleeInput()
    {
        if (!PlayerController.Instance.isMeleeOnCooldown)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                rigidBody.velocity = Vector2.zero;
                PlayerController.Instance.facing = Facing.Down;
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
        PlayerController.Instance.isInMelee = true;
        float durationCounter = 0f;

        while (durationCounter < meleeDuration)
        {
            durationCounter += Time.deltaTime;
            yield return null;
        }

        PlayerController.Instance.isInMelee = false;
    }

    public IEnumerator MeleeCooldownCounter()
    {
        PlayerController.Instance.isMeleeOnCooldown = true;
        float cooldownCounter = meleeCooldown;

        while (cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            yield return null;
        }

        PlayerController.Instance.isMeleeOnCooldown = false;
    }
}
