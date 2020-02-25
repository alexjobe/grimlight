using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public const int HEALTH_PER_HEART = 4;

    public int maxHearts = 1;

    public int currentHealth { get; private set; }

    public static PlayerHealthController Instance
    {
        get { return instance; }
    }

    void Awake()
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

    void Start()
    {
        currentHealth = maxHearts * HEALTH_PER_HEART;
    }

    void Update()
    {
        
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
