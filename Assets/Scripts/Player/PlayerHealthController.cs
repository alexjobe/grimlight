using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public const int HEALTH_PER_HEART = 4;
    public int maxHearts = 1;
    public int currentHealth { get; private set; }

    private static PlayerHealthController instance;

    public static PlayerHealthController Instance
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
        currentHealth = maxHearts * HEALTH_PER_HEART;
    }

    public void DamagePlayer(int damage = 1)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            PlayerController.Instance.gameObject.SetActive(false);
        }
    }
}
